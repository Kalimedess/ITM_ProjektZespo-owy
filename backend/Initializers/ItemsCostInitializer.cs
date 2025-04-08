
using backend.Data;


public static class ItemCostInitializer {

    public static void Initialize(AppDbContext context){
        
        // Dodawanie Przedmiotów
        var itemsCost = new List<ItemsCost>
        {
            new ItemsCost { ItemsCostId = 1, ItemsId = 1, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 2, ItemsId = 2, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 3, ItemsId = 3, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 4, ItemsId = 4, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 5, ItemsId = 5, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 6, ItemsId = 6, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 7, ItemsId = 7, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 8, ItemsId = 8, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 9, ItemsId = 9, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 10, ItemsId = 10, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 11, ItemsId = 11, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 12, ItemsId = 12, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 13, ItemsId = 13, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 14, ItemsId = 14, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 15, ItemsId = 15, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 16, ItemsId = 16, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 17, ItemsId = 17, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 18, ItemsId = 18, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 19, ItemsId = 19, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 20, ItemsId = 20, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 21, ItemsId = 21, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 22, ItemsId = 22, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 23, ItemsId = 23, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 24, ItemsId = 24, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 25, ItemsId = 25, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 26, ItemsId = 26, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 27, ItemsId = 27, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 28, ItemsId = 28, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 29, ItemsId = 29, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 30, ItemsId = 30, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 31, ItemsId = 31, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 32, ItemsId = 32, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 33, ItemsId = 33, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 34, ItemsId = 34, ItemsCostWeight = 1},
            new ItemsCost { ItemsCostId = 35, ItemsId = 35, ItemsCostWeight = 1},
        };
        foreach (var newItemCost in itemsCost)
        {
            var existingItemCost = context.ItemsCosts.FirstOrDefault(u => u.ItemsId == newItemCost.ItemsCostId);
            if (existingItemCost != null)
            {
                // Aktualizacja danych przedmiotów
                existingItemCost.ItemsCostId = newItemCost.ItemsCostId;
                existingItemCost.ItemsId = newItemCost.ItemsId;
                existingItemCost.ItemsCostWeight= newItemCost.ItemsCostWeight;
            }
            else
            {
                // Dodanie nowego przedmiotu
                context.ItemsCosts.Add(newItemCost);
            }
        }
        context.SaveChanges();
        Console.WriteLine("Zaktualizowano / dodano Koszty Przedmiotów do bazy MySQL!");
    }
}
