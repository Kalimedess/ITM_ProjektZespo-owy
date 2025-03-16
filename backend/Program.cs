using Microsoft.EntityFrameworkCore;
using backend.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:5173") //Gdyby nie działało, zobaczyć na jakim porcie działa Vue 
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("AllowFrontend");
app.MapControllers();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

// Dodaj użytkownika tylko jeśli baza jest pusta
if (!context.Users.Any())
{
    
    context.Users.Add(new User { Name = "JanKowalski", Email = "jan@example.com", Password = "jankowalski123" });
    context.Users.Add(new User { Name = "AnnaNowak", Email = "anna@example.com", Password = "annanowak987" });

    context.SaveChanges();
    Console.WriteLine("Dodano użytkowników do bazy SQLite!");
}

app.Run();