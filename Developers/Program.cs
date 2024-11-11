using Developers.Persistence;
using Developers.Persistence.InitialData;
using Developers.Repositories.Implementations;
using Developers.Repositories.Interfaces;
using Developers.Utilities;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Configurar la conexion con la base de datos
var connectionString = builder.Configuration.GetConnectionString("SqlServerConexion");
builder.Services.AddDbContext<DevelopersDbContext>(options => options.UseSqlServer(connectionString));

// Usar autenticacion con Identity
//builder.Services.AddDefaultIdentity<IdentityUser>()
//                .AddEntityFrameworkStores<DevelopersDbContext>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddDefaultUI()
                .AddEntityFrameworkStores<DevelopersDbContext>();
builder.Services.AddControllersWithViews();


// Agregar Repositorios
builder.Services.AddScoped<IUnitWork, UnitWork>();

// Servicios de Paginas de Razor
builder.Services.AddRazorPages();

// Servicio de EmailSender
builder.Services.AddSingleton<IEmailSender, EmailSender>();

// Servicio de Datos Iniciales
builder.Services.AddScoped<IDbInitialize, DbInitialize>();


var app = builder.Build();

// Datos Iniciales
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var inicializador = services.GetRequiredService<IDbInitialize>();
        inicializador.Initialize();
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "Un error ocurrió el ejecutar la migración.");
    }
    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//IMAGEN


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Usar autenticacion
app.UseAuthentication();

app.UseAuthorization();

//Clases

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//Proyecto

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Privacy}/{id?}");

//usar paginas de razor (Razor Pages)
app.MapRazorPages();

app.Run();
