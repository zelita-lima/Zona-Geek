using Microsoft.EntityFrameworkCore;
using SiteAgendamento.Repositorio;
using Zona_Geek.ORM;
using Zona_Geek.Repositorio;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Registrar o DbContext se necess�rio
builder.Services.AddDbContext<BdZonaGeekContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar o reposit�rio (UsuarioRepositorio)
builder.Services.AddScoped<UsuarioRepositorio>();
// Registrar o reposit�rio (ServicoRepositorio)
builder.Services.AddScoped<ServicoRepositorio>();
// Registrar o reposit�rio (AtendimentoRepositorio)
builder.Services.AddScoped<AtendimentoRepositorio>();
// Registrar outros servi�os, como controllers com views
builder.Services.AddControllersWithViews();

// Adicionar suporte a sess�es
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Define o tempo de expira��o da sess�o
    options.Cookie.HttpOnly = true; // Torna o cookie acess�vel apenas via HTTP
    options.Cookie.IsEssential = true; // Marca o cookie como essencial
});

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

// Adicionar o uso de sess�o
app.UseSession();  // Adicionando o middleware de sess�o

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
