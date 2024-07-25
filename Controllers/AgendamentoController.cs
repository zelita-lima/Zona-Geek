using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Zona_Geek.Models;

namespace Zona_Geek.Controllers
{
    public class AgendamentoController : Controller
    {
        private readonly ILogger<AgendamentoController> _logger;

        public AgendamentoController(ILogger<AgendamentoController> logger)
        {
            _logger = logger;
        }

        public IActionResult Agendamentoindex()
        {
            return View();
        }

        public IActionResult AgendamentoUserIndex()
        {
            return View();
        }

        public IActionResult CadastroAgendamentoCliente()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
