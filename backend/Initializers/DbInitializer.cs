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
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Plik inicjalizacyjny Excela nie został znaleziony.");
                return;
            }

            using var workbook = new XLWorkbook(filePath);
            var dbSetProps = typeof(AppDbContext).GetProperties()
                .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>)).ToList();

            var sheetOrder = new[] { "Users", "Boards", "Decks", "Processes", "Cards", "Decisions", "Items", "Feedbacks", "DecisionWeights", "DecisionEnablers" };

            Console.WriteLine("Rozpoczęto inicjalizację danych z pliku Excel...");

            foreach (var sheetName in sheetOrder)
            {
                Console.WriteLine($"Przetwarzanie arkusza: {sheetName}...");
                var sheet = workbook.Worksheets.FirstOrDefault(ws => ws.Name.Equals(sheetName, StringComparison.OrdinalIgnoreCase));
                if (sheet == null)
                {
                    Console.WriteLine($"Arkusz '{sheetName}' nie został znaleziony. Pomijanie.");
                    continue;
                }

                var dbSetProp = dbSetProps.FirstOrDefault(p => p.Name.Equals(sheet.Name, StringComparison.OrdinalIgnoreCase));
                if (dbSetProp == null) continue;

                // --- POCZĄTEK POPRAWKI ---
                var entityType = dbSetProp.PropertyType.GenericTypeArguments[0];
                var dbSetObject = dbSetProp.GetValue(context)!;
                var queryableSet = (IQueryable<object>)dbSetObject;

                if (queryableSet.Any())
                {
                    Console.WriteLine($"Dane dla '{sheetName}' już istnieją w bazie. Pomijanie.");
                    continue;
                }
                // --- KONIEC POPRAWKI ---

                var pkProp = GetPrimaryKey(context, entityType);
                var props = entityType.GetProperties().Where(p => p.CanWrite).ToList();
                var columnMap = sheet.Row(1).CellsUsed().ToDictionary(c => c.GetString().Trim(), c => c.Address.ColumnNumber);

                foreach (var row in sheet.RowsUsed().Skip(1))
                {
                    if (row.IsEmpty()) continue;
                    var instance = Activator.CreateInstance(entityType)!;

                    // Logika wczytywania wierszy (bez zmian)
                    if (pkProp != null && columnMap.TryGetValue(pkProp.Name, out var pkCol))
                    {
                        var pkStringValue = row.Cell(pkCol).GetString();
                        var primaryKeyValue = ConvertValue(pkStringValue, pkProp.PropertyType, row.Cell(pkCol).DataType);
                        if (primaryKeyValue == null || IsDefault(primaryKeyValue)) continue;
                        pkProp.SetValue(instance, primaryKeyValue);
                    }

                    foreach (var prop in props.Where(p => p != pkProp))
                    {
                        if (!columnMap.TryGetValue(prop.Name, out var col)) continue;
                        var val = ConvertValue(row.Cell(col).GetString(), prop.PropertyType, row.Cell(col).DataType);
                        if (val == null) continue;

                        if (sheetName.Equals("Users", StringComparison.OrdinalIgnoreCase) && prop.Name == "Password" && val is string password)
                        {
                            val = BCrypt.Net.BCrypt.HashPassword(password);
                        }
                        prop.SetValue(instance, val);
                    }
                    context.Add(instance);
                }

                try
                {
                    context.SaveChanges();
                    Console.WriteLine($"Pomyślnie zapisano dane z arkusza '{sheetName}'.");
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine($"BŁĄD KRYTYCZNY podczas zapisywania danych z arkusza '{sheetName}': {ex.InnerException?.Message ?? ex.Message}");
                    throw;
                }
            }
        }

        private static object? ConvertValue(string val, Type target, XLDataType type)
        {
            if (string.IsNullOrWhiteSpace(val)) return null;
            target = Nullable.GetUnderlyingType(target) ?? target;

            if (target == typeof(byte[]) && val.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                var imagePath = Path.Combine(AppContext.BaseDirectory, "Initializers", val);
                return File.Exists(imagePath) ? File.ReadAllBytes(imagePath) : null;
            }

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
