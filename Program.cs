using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using SiteAgendamento.Repositorio;
using Zona_Geek.Repositorio;
using Zona_Geek.ORM;

var builder = WebApplication.CreateBuilder(args);

// Registrar o DbContext
builder.Services.AddDbContext<BdZonaGeekContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar os reposit�rios
builder.Services.AddScoped<UsuarioRepositorio>();
builder.Services.AddScoped<ServicoRepositorio>();
builder.Services.AddScoped<AgendamentoRepositorio>();

// Registrar o IHttpContextAccessor para acessar o HttpContext dentro de reposit�rios
builder.Services.AddHttpContextAccessor();

// Adicionar suporte a sess�es
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Defina o tempo de expira��o da sess�o
    options.Cookie.HttpOnly = true; // Garante que o cookie de sess�o n�o seja acess�vel via JavaScript
    options.Cookie.IsEssential = true; // O cookie � essencial para a opera��o do site
});

// Adicionar suporte a autentica��o com cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuario/Login";  // Caminho para a p�gina de login
        options.LogoutPath = "/Usuario/Logout";  // Caminho para a p�gina de logout
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);  // Tempo de expira��o do cookie
    });

// Registrar outros servi�os, como controllers com views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configurar o pipeline de requisi��o HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Adicionar o middleware de sess�o
app.UseSession();

// Colocar a ordem correta dos middlewares de autentica��o e autoriza��o
app.UseRouting();

// Middleware de autentica��o: permite identificar quem � o usu�rio
app.UseAuthentication();  // Coloque antes de UseAuthorization()

// Middleware de autoriza��o: verifica se o usu�rio tem permiss�o para acessar a rota
app.UseAuthorization();   // Deve ser chamado depois de UseAuthentication()

// Configurar as rotas dos controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Rodar a aplica��o
app.Run();
