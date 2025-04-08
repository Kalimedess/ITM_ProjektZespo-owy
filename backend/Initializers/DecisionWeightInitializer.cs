using backend.Data;

public static class DecisionWeightInitializer {

    public static void Initialize(AppDbContext context) {

        //Dodawanie Decyzji
        var decisionWeight = new List<DecisionWeight> {
            new DecisionWeight { DecisionId = 1, WeightX = 25, WeightY = 85, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 2, WeightX = 30, WeightY = 80, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 3, WeightX = 35, WeightY = 90, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 4, WeightX = 30, WeightY = 85, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 5, WeightX = 25, WeightY = 80, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 6, WeightX = 40, WeightY = 85, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 7, WeightX = 50, WeightY = 90, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 8, WeightX = 45, WeightY = 85, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 9, WeightX = 60, WeightY = 100, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 10, WeightX = 45, WeightY = 80, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 11, WeightX = 50, WeightY = 95, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 12, WeightX = 55, WeightY = 95, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 13, WeightX = 40, WeightY = 85, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 14, WeightX = 65, WeightY = 85, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 15, WeightX = 45, WeightY = 80, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 16, WeightX = 50, WeightY = 90, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 17, WeightX = 55, WeightY = 85, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 18, WeightX = 35, WeightY = 80, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 19, WeightX = 40, WeightY = 85, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 20, WeightX = 30, WeightY = 75, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 21, WeightX = 35, WeightY = 80, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 22, WeightX = 25, WeightY = 85, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 23, WeightX = 40, WeightY = 75, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 24, WeightX = 35, WeightY = 75, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 25, WeightX = 25, WeightY = 70, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 26, WeightX = 55, WeightY = 95, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 27, WeightX = 50, WeightY = 85, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 28, WeightX = 55, WeightY = 80, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 29, WeightX = 40, WeightY = 85, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 30, WeightX = 45, WeightY = 85, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 31, WeightX = 40, WeightY = 75, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 32, WeightX = 50, WeightY = 90, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 33, WeightX = 35, WeightY = 85, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 34, WeightX = 45, WeightY = 85, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 35, WeightX = 50, WeightY = 95, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 36, WeightX = 60, WeightY = 95, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 37, WeightX = 55, WeightY = 100, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { DecisionId = 38, WeightX = 35, WeightY = 80, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 39, WeightX = 60, WeightY = 100, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 40, WeightX = 10, WeightY = 60, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 41, WeightX = 70, WeightY = 70, SubboardId = 1, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 1, WeightX = 20, WeightY = 10, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 2, WeightX = 30, WeightY = 20, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 3, WeightX = 35, WeightY = 25, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 4, WeightX = 30, WeightY = 25, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 5, WeightX = 80, WeightY = 20, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 6, WeightX = 40, WeightY = 30, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 7, WeightX = 50, WeightY = 40, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 8, WeightX = 45, WeightY = 35, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 9, WeightX = 60, WeightY = 40, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 10, WeightX = 80, WeightY = 20, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 11, WeightX = 50, WeightY = 45, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 12, WeightX = 40, WeightY = 90, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 13, WeightX = 30, WeightY = 80, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 14, WeightX = 50, WeightY = 85, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 15, WeightX = 40, WeightY = 75, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 16, WeightX = 45, WeightY = 80, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 17, WeightX = 55, WeightY = 80, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 18, WeightX = 80, WeightY = 20, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 19, WeightX = 30, WeightY = 25, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 20, WeightX = 80, WeightY = 20, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 21, WeightX = 60, WeightY = 30, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 22, WeightX = 20, WeightY = 30, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 23, WeightX = 80, WeightY = 20, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 24, WeightX = 80, WeightY = 20, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 25, WeightX = 70, WeightY = 10, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 26, WeightX = 55, WeightY = 90, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 27, WeightX = 40, WeightY = 80, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 28, WeightX = 55, WeightY = 80, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 29, WeightX = 60, WeightY = 30, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 30, WeightX = 30, WeightY = 85, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 31, WeightX = 40, WeightY = 75, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 32, WeightX = 90, WeightY = 80, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 33, WeightX = 20, WeightY = 30, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 34, WeightX = 30, WeightY = 80, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 35, WeightX = 40, WeightY = 95, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 36, WeightX = 50, WeightY = 95, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 37, WeightX = 90, WeightY = 80, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 38, WeightX = 80, WeightY = 20, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 39, WeightX = 60, WeightY = 80, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 40, WeightX = 80, WeightY = 70, SubboardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { DecisionId = 41, WeightX = 90, WeightY = 90, SubboardId = 2, BoosterX = 1, BoosterY = 1},
        };

        foreach (var newDecisionWeight in decisionWeight)
        {
            var existingDecisionWeight = context.DecisionWeights.FirstOrDefault(dw => dw.DecisionId == newDecisionWeight.DecisionId);
            if (existingDecisionWeight != null)
            {
                // Aktualizacja danych użytkownika
                existingDecisionWeight.DecisionId = newDecisionWeight.DecisionId;
                existingDecisionWeight.WeightX = newDecisionWeight.WeightX;
                existingDecisionWeight.WeightY = newDecisionWeight.WeightY;
                existingDecisionWeight.SubboardId = newDecisionWeight.SubboardId;
                existingDecisionWeight.BoosterX = newDecisionWeight.BoosterX;
                existingDecisionWeight.BoosterY = newDecisionWeight.BoosterY;
            }
            else
            {
                // Dodanie nowego użytkownika
                context.DecisionWeights.Add(newDecisionWeight);
            }
        }
        context.SaveChanges();
        Console.WriteLine("Zaktualizowano / dodano Koszty Decyzji do bazy MySQL!");
    }
}