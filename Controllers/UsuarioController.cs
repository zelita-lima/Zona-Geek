using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using Zona_Geek.Models;
using Zona_Geek.Repositorio;
using Microsoft.AspNetCore.Authorization;

namespace Zona_Geek.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioRepositorio _usuarioRepositorio;
        private readonly ILogger<UsuarioController> _logger;
        public UsuarioController(UsuarioRepositorio usuarioRepositorio, ILogger<UsuarioController> logger)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _logger = logger;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerificarUsuario(LoginViewModel model)
        {
            try
            {
                // Verifica se o modelo � v�lido
                if (!ModelState.IsValid)
                {
                    // Retorna � view de login com o modelo, incluindo os erros de valida��o, se houver
                    return View("Login", model); // Especifica a view "Login"
                }

                // Verifica se o usu�rio existe com as credenciais fornecidas
                var usuario = _usuarioRepositorio.VerificarLogin(model.Email, model.Senha);

                if (usuario != null)
                {
                    // Cria��o das claims do usu�rio para o cookie de autentica��o
                    var claims = new List<Claim>
     {
         new Claim(ClaimTypes.Name, usuario.Nome),
         new Claim(ClaimTypes.Email, usuario.Email),
         // Voc� pode adicionar outras claims, como roles ou permiss�es, se necess�rio
     };

                    // Cria��o da identidade do usu�rio para o cookie de autentica��o
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Cria��o do principal (usu�rio autenticado)
                    var principal = new ClaimsPrincipal(identity);

                    // Autentica��o do usu�rio e cria��o do cookie
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    // Redireciona para a p�gina inicial ou outra p�gina que voc� preferir
                    return RedirectToAction("Index", "Home");  // Redirecionamento para a p�gina inicial
                }
                else
                {
                    // Caso o usu�rio n�o seja encontrado, exibe uma mensagem de erro
                    ViewData["ErrorMessage"] = "E-mail ou senha inv�lidos.";
                    Debug.WriteLine("Erro: E-mail ou senha inv�lidos.");

                    // Redireciona para a view "Login", onde a mensagem de erro ser� exibida
                    return View("Login", model); // Retorna para a view Login com o modelo
                }
            }
            catch (Exception ex)
            {
                // Em caso de erro inesperado, captura o erro e exibe a mensagem
                ViewData["ErrorMessage"] = "Erro ao processar a solicita��o. Detalhes: " + ex.Message;
                return View("Login", model); // Retorna � mesma view "Login" com a mensagem de erro
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var csrfToken = Request.Form["__RequestVerificationToken"];
                if (string.IsNullOrEmpty(csrfToken))
                {
                    throw new Exception("Token CSRF ausente");
                }

                // Limpa o cookie de autentica��o
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                // Limpar a sess�o
                HttpContext.Session.Clear();  // Limpa todos os dados de sess�o armazenados

                // Limpar os cookies de sess�o
                foreach (var cookie in Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookie);  // Deleta todos os cookies
                }

                // Evitar cache de p�ginas protegidas
                Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
                Response.Headers["Pragma"] = "no-cache";
                Response.Headers["Expires"] = "0";

                // Verificar se � uma requisi��o AJAX
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true });
                }
                else
                {
                    return RedirectToAction("Login", "Account");  // Redireciona para a p�gina de login
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public IActionResult Cadastro()
        {
            return View();

        }
        public IActionResult Login()
        {
            return View(new LoginViewModel());

        }
        [Authorize]
        public IActionResult Index()
        {
            List<SelectListItem> tipoUsuario = new List<SelectListItem>
             {
                 new SelectListItem { Value = "1", Text = "Administrador" },
                 new SelectListItem { Value = "0", Text = "Cliente" }
             };

            ViewBag.lstTipoUsuario = new SelectList(tipoUsuario, "Value", "Text");

            var Usuarios = _usuarioRepositorio.ListarUsuarios();
            return View(Usuarios);
        }
        public IActionResult InserirUsuario(string Nome, string Senha, string Email, string Telefone, int TipoUsuario)
        {
            try
            {
                // Chama o m�todo do reposit�rio que realiza a inser��o no banco de dados
                var resultado = _usuarioRepositorio.InserirUsuario(Nome, Email, Telefone, Senha,  TipoUsuario);

                // Verifica o resultado da inser��o
                if (resultado)
                {
                    // Se o resultado for verdadeiro, significa que o usu�rio foi inserido com sucesso
                    return Json(new { success = true, message = "Cliente inserido com sucesso!" });
                }
                else
                {
                    // Se o resultado for falso, significa que houve um erro ao tentar inserir o usu�rio
                    return Json(new { success = false, message = "Erro ao inserir o cliente. Tente novamente." });
                }
            }
            catch (Exception ex)
            {
                // Em caso de erro inesperado, captura e exibe o erro
                return Json(new { success = false, message = "Erro ao processar a solicita��o. Detalhes: " + ex.Message });
            }
        }
        public IActionResult AtualizarUsuario(int id, string Nome, string Senha, string Email, string Telefone, int TipoUsuario)
        {
            try
            {
                // Chama o reposit�rio para atualizar o usu�rio
                var resultado = _usuarioRepositorio.AtualizarUsuario(id, Nome, Email, Telefone, Senha, TipoUsuario);

                if (resultado)
                {
                    return Json(new { success = true, message = "Usu�rio atualizado com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Erro ao atualizar o usu�rio. Verifique se o usu�rio existe." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erro ao processar a solicita��o. Detalhes: " + ex.Message });
            }
        }
        public IActionResult ExcluirUsuario(int id)
        {
            try
            {
                // Chama o reposit�rio para excluir o usu�rio
                var resultado = _usuarioRepositorio.ExcluirUsuario(id);

                if (resultado)
                {
                    return Json(new { success = true, message = "Usu�rio exclu�do com sucesso!" });
                }
                else
                {
                    // Se o resultado for falso, voc� pode fornecer uma mensagem mais espec�fica.
                    return Json(new { success = false, message = "N�o foi poss�vel excluir o usu�rio. Verifique se ele est� vinculado a outros registros no sistema." });
                }
            }
            catch (Exception ex)
            {
                // Captura qualquer erro e inclui a mensagem detalhada da exce��o
                return Json(new { success = false, message = "Erro ao processar a solicita��o. Detalhes: " + ex.Message });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

