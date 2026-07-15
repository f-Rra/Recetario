using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using RecetarioMVC.Data;
using RecetarioMVC.Models;

var builder = WebApplication.CreateBuilder(args);

// Cultura es-AR en toda la app: decimales con coma en binding y formato (guía 07)
var culturaArgentina = new CultureInfo("es-AR");
CultureInfo.DefaultThreadCurrentCulture = culturaArgentina;
CultureInfo.DefaultThreadCurrentUICulture = culturaArgentina;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RecetarioMVC")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        // Sin registro público: los usuarios los crea el Admin (guía 04)
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.User.RequireUniqueEmail = true;
        options.Lockout.MaxFailedAccessAttempts = 5;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddErrorDescriber<RecetarioMVC.Helpers.IdentityErrorDescriberEspanol>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Acceso/Login";
    options.AccessDeniedPath = "/Acceso/Denegado";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
});

builder.Services.AddScoped<RecetarioMVC.Services.IDashboardService, RecetarioMVC.Services.DashboardService>();
builder.Services.AddScoped<RecetarioMVC.Services.IIngredienteService, RecetarioMVC.Services.IngredienteService>();
builder.Services.AddScoped<RecetarioMVC.Services.IProveedorService, RecetarioMVC.Services.ProveedorService>();
builder.Services.AddScoped<RecetarioMVC.Services.IPrecioIngredienteService, RecetarioMVC.Services.PrecioIngredienteService>();
builder.Services.AddScoped<RecetarioMVC.Services.IRecetaService, RecetarioMVC.Services.RecetaService>();
builder.Services.AddScoped<RecetarioMVC.Services.ICosteoService, RecetarioMVC.Services.CosteoService>();
builder.Services.AddScoped<RecetarioMVC.Services.IComandaService, RecetarioMVC.Services.ComandaService>();
builder.Services.AddScoped<RecetarioMVC.Services.IPersonaService, RecetarioMVC.Services.PersonaService>();
builder.Services.AddScoped<RecetarioMVC.Services.IStockService, RecetarioMVC.Services.StockService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(culturaArgentina),
    SupportedCultures = [culturaArgentina],
    SupportedUICultures = [culturaArgentina]
});

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    await DbSeeder.SeedAsync(scope.ServiceProvider);
}

app.Run();
