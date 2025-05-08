
using backend.Data;


public static class UsersInitializer {

    public static void Initialize(AppDbContext context){
        
        // Dodawanie Użytkowników
        var users = new List<User>
        {
            new User { Name = "JanKowalski", Email = "jan@example.com", Password = BCrypt.Net.BCrypt.HashPassword("jankowalski123"), EmailConfirmed = true, LicensesOwned = 255 },
            new User { Name = "AnnaNowak", Email = "anna@example.com", Password = BCrypt.Net.BCrypt.HashPassword("annanowak987"), EmailConfirmed = true, LicensesOwned = 255 }
        };
        foreach (var newUser in users)
        {
            var existingUser = context.Users.FirstOrDefault(u => u.Email == newUser.Email);
            if (existingUser != null)
            {
                // Aktualizacja danych użytkownika
                existingUser.Name = newUser.Name;
                existingUser.Password = newUser.Password;
                existingUser.EmailConfirmed = newUser.EmailConfirmed;
                existingUser.LicensesOwned = newUser.LicensesOwned;
            }
            else
            {
                // Dodanie nowego użytkownika
                context.Users.Add(newUser);
            }
        }
        context.SaveChanges();
        Console.WriteLine("Zaktualizowano / dodano Użytkowników do bazy MySQL!");
    }
}
