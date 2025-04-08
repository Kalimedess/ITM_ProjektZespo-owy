using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Services;
using backend.Initializers;

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

using (var scope = app.Services.CreateScope()) {
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbInitializer.Initialize(context);
}

app.Run();