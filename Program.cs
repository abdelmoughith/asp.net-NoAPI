using Authentication_Client.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000); // HTTP
    options.ListenAnyIP(8443, listenOptions => listenOptions.UseHttps()); // HTTPS
});





// Configure the database connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=localhost;Database=hotelDb;User Id=SA;Password=Db@12345;TrustServerCertificate=True;")
);
// Configure JWT authentication

builder.Services.AddScoped<JwtTokenService>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
    };
});

// Configure Authorization policies (optional: for role or claim-based access)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Admin")); // Require 'Admin' role for specific endpoints

    options.AddPolicy("UserPolicy", policy =>
        policy.RequireClaim("UserAccess", "Allowed")); // Custom claim-based policy
});




// Add controllers to the services container
// builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    
    app.UseExceptionHandler("/error");
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseCors(builder =>
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers(); // Maps API controllers

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Auth}/{action=Register}/{id?}")
    .WithStaticAssets();

app.Run();
