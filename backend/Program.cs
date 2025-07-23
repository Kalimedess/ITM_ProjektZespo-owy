using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Services;
using backend.Initializers;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load(Path.Combine(Directory.GetCurrentDirectory(), "../.env"));

// Pobierz URL frontendu ze zmiennej środowiskowej
var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL");

if (!string.IsNullOrEmpty(frontendUrl))
{
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins(frontendUrl)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
    });
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
                     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));



var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();

builder.Services.AddSingleton(emailSettings);
builder.Services.AddScoped<EmailService>();
builder.Services.AddControllers();
builder.Services.AddScoped<JwtService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.Cookie.Name = "itm.auth.cookie";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
            ? CookieSecurePolicy.None
            : CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;

        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;

        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
       
       builder.Services.AddSignalR();         context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<GameHub>("/gameHub");



using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    DbInitializer.Initialize(context);
}

app.Run();