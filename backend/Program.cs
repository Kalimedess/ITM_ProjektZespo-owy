using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:5173") //Gdyby nie działało, zobaczyć na jakim porcie działa Vue 
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
                     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));



var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();
builder.Services.AddSingleton(emailSettings);
builder.Services.AddControllers();
builder.Services.AddScoped<EmailService>();


var app = builder.Build();

app.UseCors("AllowFrontend");
app.MapControllers();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

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
Console.WriteLine("Zaktualizowano / dodano użytkowników do bazy SQLite!");

app.Run();