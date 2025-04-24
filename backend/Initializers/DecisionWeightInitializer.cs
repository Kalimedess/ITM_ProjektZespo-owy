using backend.Data;

public static class DecisionWeightInitializer {

    public static void Initialize(AppDbContext context) {

        //Dodawanie Decyzji
        var decisionWeight = new List<DecisionWeight> {
            new DecisionWeight { CardId = 1, WeightX = 25, WeightY = 85, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 2, WeightX = 30, WeightY = 80, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 3, WeightX = 35, WeightY = 90, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 4, WeightX = 30, WeightY = 85, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 5, WeightX = 25, WeightY = 80, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 6, WeightX = 40, WeightY = 85, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 7, WeightX = 50, WeightY = 90, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 8, WeightX = 45, WeightY = 85, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 9, WeightX = 60, WeightY = 100, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 10, WeightX = 45, WeightY = 80, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 11, WeightX = 50, WeightY = 95, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 12, WeightX = 55, WeightY = 95, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 13, WeightX = 40, WeightY = 85, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 14, WeightX = 65, WeightY = 85, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 15, WeightX = 45, WeightY = 80, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 16, WeightX = 50, WeightY = 90, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 17, WeightX = 55, WeightY = 85, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 18, WeightX = 35, WeightY = 80, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 19, WeightX = 40, WeightY = 85, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 20, WeightX = 30, WeightY = 75, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 21, WeightX = 35, WeightY = 80, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 22, WeightX = 25, WeightY = 85, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 23, WeightX = 40, WeightY = 75, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 24, WeightX = 35, WeightY = 75, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 25, WeightX = 25, WeightY = 70, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 26, WeightX = 55, WeightY = 95, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 27, WeightX = 50, WeightY = 85, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 28, WeightX = 55, WeightY = 80, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 29, WeightX = 40, WeightY = 85, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 30, WeightX = 45, WeightY = 85, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 31, WeightX = 40, WeightY = 75, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 32, WeightX = 50, WeightY = 90, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 33, WeightX = 35, WeightY = 85, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 34, WeightX = 45, WeightY = 85, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 35, WeightX = 50, WeightY = 95, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 36, WeightX = 60, WeightY = 95, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 37, WeightX = 55, WeightY = 100, BoardId = 1, BoosterX = 1, BoosterY = 1},
            new DecisionWeight { CardId = 38, WeightX = 35, WeightY = 80, BoardId = 1, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 39, WeightX = 60, WeightY = 100, BoardId = 1, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 40, WeightX = 10, WeightY = 60, BoardId = 1, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 41, WeightX = 70, WeightY = 70, BoardId = 1, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 1, WeightX = 20, WeightY = 10, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 2, WeightX = 30, WeightY = 20, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 3, WeightX = 35, WeightY = 25, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 4, WeightX = 30, WeightY = 25, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 5, WeightX = 80, WeightY = 20, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 6, WeightX = 40, WeightY = 30, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 7, WeightX = 50, WeightY = 40, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 8, WeightX = 45, WeightY = 35, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 9, WeightX = 60, WeightY = 40, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 10, WeightX = 80, WeightY = 20, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 11, WeightX = 50, WeightY = 45, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 12, WeightX = 40, WeightY = 90, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 13, WeightX = 30, WeightY = 80, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 14, WeightX = 50, WeightY = 85, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 15, WeightX = 40, WeightY = 75, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 16, WeightX = 45, WeightY = 80, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 17, WeightX = 55, WeightY = 80, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 18, WeightX = 80, WeightY = 20, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 19, WeightX = 30, WeightY = 25, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 20, WeightX = 80, WeightY = 20, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 21, WeightX = 60, WeightY = 30, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 22, WeightX = 20, WeightY = 30, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 23, WeightX = 80, WeightY = 20, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 24, WeightX = 80, WeightY = 20, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 25, WeightX = 70, WeightY = 10, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 26, WeightX = 55, WeightY = 90, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 27, WeightX = 40, WeightY = 80, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 28, WeightX = 55, WeightY = 80, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 29, WeightX = 60, WeightY = 30, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 30, WeightX = 30, WeightY = 85, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 31, WeightX = 40, WeightY = 75, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 32, WeightX = 90, WeightY = 80, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 33, WeightX = 20, WeightY = 30, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 34, WeightX = 30, WeightY = 80, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 35, WeightX = 40, WeightY = 95, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 36, WeightX = 50, WeightY = 95, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 37, WeightX = 90, WeightY = 80, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 38, WeightX = 80, WeightY = 20, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 39, WeightX = 60, WeightY = 80, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 40, WeightX = 80, WeightY = 70, BoardId = 2, BoosterX = 1, BoosterY = 1},
            // new DecisionWeight { CardId = 41, WeightX = 90, WeightY = 90, BoardId = 2, BoosterX = 1, BoosterY = 1},
        };

        foreach (var newDecisionWeight in decisionWeight)
        {
            var existingDecisionWeight = context.DecisionWeights.FirstOrDefault(dw => dw.CardId == newDecisionWeight.CardId);
            if (existingDecisionWeight != null)
            {
                // Aktualizacja danych użytkownika
                existingDecisionWeight.CardId = newDecisionWeight.CardId;
                existingDecisionWeight.WeightX = newDecisionWeight.WeightX;
                existingDecisionWeight.WeightY = newDecisionWeight.WeightY;
                existingDecisionWeight.BoardId = newDecisionWeight.BoardId;
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