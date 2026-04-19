using Microsoft.EntityFrameworkCore;
using Proyecto1.BLL;
using Proyecto1.DAL;
using Proyecto1.Services;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<ColaboradoresService>();
builder.Services.AddScoped<AsistenciaService>();
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
builder.Services.AddScoped<ExcelService>();

builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ?? SIMPLE: Redirigir 404 a Error
app.MapFallback(context =>
{
    context.Response.Redirect("/Home/Error?statusCode=404");
    return Task.CompletedTask;
});

app.Run();