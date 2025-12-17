using Microsoft.EntityFrameworkCore;
using Proyecto1.DAL;
using Proyecto1.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// EF SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Sesiones
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// Servicio de login
builder.Services.AddScoped<LoginService>();

builder.Services.AddScoped<ColaboradoresService>();


var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();

