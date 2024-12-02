using Microsoft.EntityFrameworkCore;
using SiteAgendamento.Repositorio;
using Zona_Geek.ORM;
using Zona_Geek.Repositorio;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Registrar o DbContext se necessário
builder.Services.AddDbContext<BdZonaGeekContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar o repositório (UsuarioRepositorio)
builder.Services.AddScoped<UsuarioRepositorio>();
// Registrar o repositório (UsuarioRepositorio)
builder.Services.AddScoped<ServicoRepositorio>();
// Registrar o repositório (UsuarioRepositorio)
builder.Services.AddScoped<AtendimentoRepositorio>();
// Registrar outros serviços, como controllers com views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
