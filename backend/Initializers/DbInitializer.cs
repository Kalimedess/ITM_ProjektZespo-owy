using backend.Data;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection;

namespace backend.Initializers
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.Migrate();
            LoadData(context);
        }

        private static void LoadData(AppDbContext context)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "Initializers", "DigitalWars_InicjalizacjaBazyDanych.xlsx");
            if (!File.Exists(filePath)) return;

            using var workbook = new XLWorkbook(filePath);
            var dbSetProps = typeof(AppDbContext).GetProperties().Where(p => p.PropertyType.Name.StartsWith("DbSet")).ToList();
            var sheetOrder = new[] { "Users", "Boards", "Decks", "GameProcess", "Cards", "Decisions", "Items", "Feedbacks", "DecisionWeights", "DecisionEnablers" };
            var trackedCards = new Dictionary<int, Card>();

            foreach (var name in sheetOrder)
            {
                var sheet = workbook.Worksheets.FirstOrDefault(ws => ws.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (sheet == null) continue;

                var dbSetProp = dbSetProps.FirstOrDefault(p => p.Name.Equals(sheet.Name, StringComparison.OrdinalIgnoreCase));
                if (dbSetProp == null) continue;

                var entityType = dbSetProp.PropertyType.GenericTypeArguments[0];
                var dbSet = dbSetProp.GetValue(context);
                var pk = GetPrimaryKey(context, entityType);
                var props = entityType.GetProperties().Where(p => p.CanWrite).ToList();
                var map = sheet.Row(1).CellsUsed().ToDictionary(c => c.GetString().Trim(), c => c.Address.ColumnNumber);

                foreach (var row in sheet.RowsUsed().Skip(1))
                {
                    if (row.IsEmpty()) continue;
                    var instance = Activator.CreateInstance(entityType)!;
                    var hasData = false;
                    object? pkVal = null;

                    if (pk != null && map.TryGetValue(pk.Name, out var pkCol))
                    {
                        pkVal = ConvertValue(row.Cell(pkCol).GetString(), pk.PropertyType, row.Cell(pkCol).DataType, name);
                        if (pkVal == null || IsDefault(pkVal)) continue;
                        pk.SetValue(instance, pkVal);
                        hasData = true;
                    }

                    foreach (var prop in props.Where(p => p != pk))
                    {
                        if (!map.TryGetValue(prop.Name, out var col)) continue;
                        var val = ConvertValue(row.Cell(col).GetString(), prop.PropertyType, row.Cell(col).DataType, name);
                        if (val == null) continue;
                        prop.SetValue(instance, val);
                        hasData = true;
                    }

                    if (!hasData || Exists(context, dbSet, entityType, pk, pkVal)) continue;

                    if (entityType == typeof(DecisionEnabler) && instance is DecisionEnabler de)
                    {
                        if (!trackedCards.ContainsKey(de.CardId) || !trackedCards.ContainsKey(de.EnablerId)) continue;
                    }

                    if (entityType == typeof(DecisionWeight) && instance is DecisionWeight dw)
                    {
                        if (!trackedCards.ContainsKey(dw.CardId)) continue;
                    }

                    dbSetProp.PropertyType.GetMethod("Add")?.Invoke(dbSet, new[] { instance });

                    if (entityType == typeof(Card) && pkVal is int id && !trackedCards.ContainsKey(id))
                        trackedCards[id] = instance as Card;
                }
            }

            try { context.SaveChanges(); }
            catch (Exception ex) { Console.WriteLine("Błąd zapisu: " + ex.Message); }
        }

        private static object? ConvertValue(string val, Type target, XLDataType type, string sheet)
        {
            if (string.IsNullOrWhiteSpace(val)) return null;
            target = Nullable.GetUnderlyingType(target) ?? target;

            return target switch
            {
                Type t when t == typeof(string) => val,
                Type t when t == typeof(int) && int.TryParse(val, out var i) => i,
                Type t when t == typeof(long) && long.TryParse(val, out var l) => l,
                Type t when t == typeof(double) && double.TryParse(val.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var d) => d,
                Type t when t == typeof(decimal) && decimal.TryParse(val.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var m) => m,
                Type t when t == typeof(bool) => val.Trim().ToLower() switch
                {
                    "1" or "true" => true,
                    "0" or "false" => false,
                    _ => null
                },
                Type t when t == typeof(DateTime) => type == XLDataType.DateTime || type == XLDataType.Number
                    ? DateTime.FromOADate(double.Parse(val, CultureInfo.InvariantCulture))
                    : DateTime.TryParse(val, out var dt) ? dt : null,
                Type t when t.IsEnum => Enum.TryParse(t, val, true, out var enumVal) ? enumVal : Enum.GetValues(t).GetValue(0),
                Type t when t == typeof(byte[]) && sheet == "Feedbacks" =>
                    File.Exists(Path.Combine(AppContext.BaseDirectory, "Initializers", val))
                        ? File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "Initializers", val))
                        : null,
                _ => null
            };
        }

        private static PropertyInfo? GetPrimaryKey(AppDbContext context, Type type) =>
            context.Model.FindEntityType(type)?.FindPrimaryKey()?.Properties.FirstOrDefault()?.PropertyInfo;

        private static bool Exists(AppDbContext context, dynamic dbSet, Type type, PropertyInfo? pk, object? val)
        {
            if (pk == null || val == null) return false;
            return context.ChangeTracker.Entries().Any(e => e.Entity.GetType() == type && val.Equals(pk.GetValue(e.Entity)))
                || dbSet.Find(new object[] { val }) != null;
        }

        private static bool IsDefault(object value) =>
            value.Equals(Activator.CreateInstance(value.GetType()));
    }
}
