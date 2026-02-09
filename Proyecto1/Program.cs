using Microsoft.EntityFrameworkCore;
using Proyecto1.BLL;
using Proyecto1.DAL;
using Proyecto1.Services;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servicios
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<ColaboradoresService>();
builder.Services.AddScoped<AsistenciaService>();
builder.Services.AddScoped<AsistenciaAuditoriaService>();
builder.Services.AddScoped<HorasExtrasService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<PermisosService>();
builder.Services.AddScoped<VacacionesService>();
builder.Services.AddScoped<IncapacidadesService>();
builder.Services.AddScoped<PlanillasService>();
builder.Services.AddScoped<AguinaldoService>();
builder.Services.AddScoped<LiquidacionService>();
builder.Services.AddScoped<UniformeService>();
builder.Services.AddScoped<EvaluacionService>();




// Session (si la usas)
builder.Services.AddSession();

var app = builder.Build();

app.UseSession();
app.UseRouting();

app.MapDefaultControllerRoute();
app.Run();

