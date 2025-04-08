using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Initializers{

    public static class DbInitializer {

        public static void Initialize(AppDbContext context) {
            context.Database.Migrate();

            DeckInitializer.Initialize(context);
            DecisionInitializer.Initialize(context);
            BoardInitializer.Initialize(context);
            UsersInitializer.Initialize(context);
            
            ItemInitializer.Initialize(context);
            ItemCostInitializer.Initialize(context);
            DecisionWeightInitializer.Initialize(context);
            DecisionCostInitializer.Initialize(context);
            FeedbackInitializer.Initialize(context);

        }
    }

}
