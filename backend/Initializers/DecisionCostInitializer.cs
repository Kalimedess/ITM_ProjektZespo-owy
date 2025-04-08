using backend.Data;

public static class DecisionCostInitializer {

    public static void Initialize(AppDbContext context) {

        //Dodawanie Decyzji
        var decisionCosts = new List<DecisionCost> {
            new DecisionCost { DecisionId = 1, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 2, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 3, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 4, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 5, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 6, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 7, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 8, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 9, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 10, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 11, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 12, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 13, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 14, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 15, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 16, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 17, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 18, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 19, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 20, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 21, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 22, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 23, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 24, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 25, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 26, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 27, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 28, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 29, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 30, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 31, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 32, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 33, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 34, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 35, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 36, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 37, DecisionCostWeight = 1},
            new DecisionCost { DecisionId = 38, DecisionCostWeight = 1},
        };

        foreach (var newDecisionCost in decisionCosts)
        {
            var existingDecisionCost = context.DecisionCosts.FirstOrDefault(dc => dc.DecisionId == newDecisionCost.DecisionId);
            if (existingDecisionCost != null)
            {
                // Aktualizacja danych użytkownika
                existingDecisionCost.DecisionId = newDecisionCost.DecisionId;
                existingDecisionCost.DecisionCostWeight = newDecisionCost.DecisionCostWeight;
            }
            else
            {
                // Dodanie nowego użytkownika
                context.DecisionCosts.Add(newDecisionCost);
            }
        }
        context.SaveChanges();
        Console.WriteLine("Zaktualizowano / dodano Koszty Decyzji do bazy MySQL!");
    }
}