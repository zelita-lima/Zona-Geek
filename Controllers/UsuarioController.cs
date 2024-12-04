using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Zona_Geek.Models;
using Zona_Geek.Repositorio;

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
        public IActionResult VerificarUsuario(string email, string senha)
        {
            try
            {
                // Verifica se os par�metros foram passados corretamente
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
                {
                    return Json(new { success = false, message = "E-mail e senha s�o necess�rios." });
                }

                // Chama o m�todo do reposit�rio para verificar o usu�rio baseado no e-mail e senha
                var usuario = _usuarioRepositorio.VerificarLogin(email, senha);

                // Verifica se o usu�rio foi encontrado e autenticado
                if (usuario != null)
                {
                    // Se o usu�rio foi encontrado, significa que o login foi bem-sucedido
                    return Json(new { success = true, message = "Usu�rio autenticado com sucesso!" });
                }
                else
                {
                    // Se n�o encontrar o usu�rio, a resposta � de falha na autentica��o
                    return Json(new { success = false, message = "E-mail ou senha inv�lidos." });
                }
            }
            catch (Exception ex)
            {
                // Em caso de erro inesperado, captura e exibe o erro
                return Json(new { success = false, message = "Erro ao processar a solicita��o. Detalhes: " + ex.Message });
            }
        }
        public IActionResult Cadastro()
        {
            return View();

        }
        public IActionResult Login()
        {
            return View();

        }
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
        public IActionResult Logout()
        {
            try
            {
                // Limpar a sess�o
                HttpContext.Session.Clear();

                // Limpar as vari�veis de ambiente
                Environment.SetEnvironmentVariable("USUARIO_ID", null);
                Environment.SetEnvironmentVariable("USUARIO_NOME", null);
                Environment.SetEnvironmentVariable("USUARIO_EMAIL", null);
                Environment.SetEnvironmentVariable("USUARIO_TELEFONE", null);
                Environment.SetEnvironmentVariable("USUARIO_TIPO", null);

                // Caso esteja usando o SignInManager, tamb�m deve chamar o m�todo de logout
                // _signInManager.SignOutAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Se ocorrer um erro, retornar uma resposta de erro com a mensagem
                return Json(new { success = false, message = ex.Message });
            }
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

