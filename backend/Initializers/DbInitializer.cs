using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Initializers{

    public static class DbInitializer {

        public static void Initialize(AppDbContext context) {
            context.Database.Migrate();

            DeckInitializer.Initialize(context);
            CardInitializer.Initialize(context);
            DecisionInitializer.Initialize(context);
            BoardInitializer.Initialize(context);
            UsersInitializer.Initialize(context);
            ItemInitializer.Initialize(context);
            DecisionWeightInitializer.Initialize(context);
            FeedbackInitializer.Initialize(context);
            DecisionEnablerInitializer.Initialize(context);
            
        }
    }

}
