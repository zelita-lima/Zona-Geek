using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SiteAgendamento.Repositorio;
using System.Diagnostics;
using Zona_Geek.Models;

namespace Zona_Geek.Controllers
{
    public class AgendamentoController : Controller
    {
        private readonly AtendimentoRepositorio _atendimentoRepositorio;
        private readonly ILogger<AgendamentoController> _logger;
        public AgendamentoController(AtendimentoRepositorio agendamentoRepositorio, ILogger<AgendamentoController> logger)
        {
            _atendimentoRepositorio = agendamentoRepositorio;
            _logger = logger;
        }

        public IActionResult Agendamentoindex()
        {
            return View();
        }
        public IActionResult GerencimentoAgendamentoUsuario()
        {
            return View();
        }
        public IActionResult CadastroAgendamento()
        {
            return View();
        }
        public IActionResult Index()
        {
            // Criar a lista de SelectListItems, onde o 'Value' será o 'Id' e o 'Text' será o 'TipoServico'
            List<SelectListItem> tipoServico = new List<SelectListItem>
              {
                  new SelectListItem { Value = "0", Text = "Consultoria em TI" },
                  new SelectListItem { Value = "1", Text = "Desenvolvimento de Software" },
                   new SelectListItem { Value = "2", Text = "Suporte Técnico" },
                  new SelectListItem { Value = "3", Text = "Treinamento Corporativo" },
                   new SelectListItem { Value = "4", Text = "Auditoria de Sistemas" },
                  new SelectListItem { Value = "5", Text = "Implementação de ERP" }
              };

            // Passar a lista para a View usando ViewBag
            ViewBag.lstTipoServico = new SelectList(tipoServico, "Value", "Text");
            var atendimentos = _atendimentoRepositorio.ListarAtendimentos();
            return View(atendimentos);
        }

        public IActionResult InserirAtendimento(int id, DateTime DtHorarioAgendamento, DateOnly DataAtendimento, TimeOnly Horario, int fk_Usuario, int fk_Servico)
        {
            try
            {
                // Chama o método do repositório para realizar a inserção no banco de dados
                var resultado = _atendimentoRepositorio.InserirAtendimento(id, DtHorarioAgendamento, DataAtendimento, Horario, fk_Usuario, fk_Servico);

                // Verifica o resultado da inserção
                if (resultado)
                {
                    return Json(new { success = true, message = "Atendimento inserido com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Erro ao inserir o atendimento. Tente novamente." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erro ao processar a solicitação. Detalhes: " + ex.Message });
            }
        }
        public IActionResult AtualizarAtendimento(int id, DateTime DtHorarioAgendamento, DateOnly DataAtendimento, TimeOnly Horario, int fk_Usuario, int fk_Servico)
        {
            try
            {
                // Chama o método do repositório para atualizar o atendimento
                var resultado = _atendimentoRepositorio.AtualizarAtendimento(id, DtHorarioAgendamento, DataAtendimento, Horario, fk_Usuario, fk_Servico);

                if (resultado)
                {
                    return Json(new { success = true, message = "Atendimento atualizado com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Erro ao atualizar o atendimento. Verifique se o atendimento existe." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erro ao processar a solicitação. Detalhes: " + ex.Message });
            }
        }
        public IActionResult ExcluirAtendimento(int id)
        {
            try
            {
                // Chama o repositório para excluir o atendimento
                var resultado = _atendimentoRepositorio.ExcluirAtendimento(id);

                if (resultado)
                {
                    return Json(new { success = true, message = "Atendimento excluído com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Erro ao excluir o atendimento. Verifique se ele está vinculado a outros registros no sistema." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erro ao processar a solicitação. Detalhes: " + ex.Message });
            }
        }

        public IActionResult ConsultarAgendamento(string data)
        {

            var agendamento = _atendimentoRepositorio.ConsultarAgendamento(data);

            if (agendamento != null)
            {
                return Json(agendamento);
            }
            else
            {
                return NotFound();
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
