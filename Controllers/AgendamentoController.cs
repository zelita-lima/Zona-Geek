using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SiteAgendamento.Repositorio;
using System.Diagnostics;
using System.Globalization;
using Zona_Geek.Models;
using Zona_Geek.ORM;

namespace Zona_Geek.Controllers
{
    public class AgendamentoController : Controller
    {
        private readonly AgendamentoRepositorio _agendamentoRepositorio;
        private BdZonaGeekContext _context;
        public AgendamentoController(AgendamentoRepositorio agendamentoRepositorio, BdZonaGeekContext context)
        {
            _agendamentoRepositorio = agendamentoRepositorio;
            _context = context;
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
            var servicos = new ServicoRepositorio(_context);
            var nomeServicos = servicos.ListarNomesServicos();
            if (nomeServicos != null && nomeServicos.Any())
            {
                // Cria a lista de SelectListItem
                var selectList = nomeServicos.Select(u => new SelectListItem
                {
                    Value = u.id.ToString(),  // O valor do item será o ID do usuário
                    Text = u.TipoServico             // O texto exibido será o nome do usuário
                }).ToList();

                // Passa a lista para o ViewBag para ser utilizada na view
                ViewBag.lstTipoServico = selectList;
            }
            // Chama o método ListarNomesAgendamentos para obter a lista de usuários
            var usuarios = _agendamentoRepositorio.ListarNomesAgendamentos();

            if (usuarios != null && usuarios.Any())
            {
                // Cria a lista de SelectListItem
                var selectList = usuarios.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),  // O valor do item será o ID do usuário
                    Text = u.Nome             // O texto exibido será o nome do usuário
                }).ToList();

                // Passa a lista para o ViewBag para ser utilizada na view
                ViewBag.Usuarios = selectList;
            }
            var atendimentos = _agendamentoRepositorio.ListarAgendamentos();
            return View(atendimentos);

        }
        public IActionResult InserirAgendamento(DateTime dtHoraAgendamento, DateOnly dataAtendimento, TimeOnly horario, int fkUsuarioId, int fkServicoId)
        {
            try
            {
                // Chama o método do repositório que realiza a inserção no banco de dados
                var resultado = _agendamentoRepositorio.InserirAgendamento(dtHoraAgendamento, dataAtendimento, horario, fkUsuarioId, fkServicoId);

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
                // Em caso de erro inesperado, captura e exibe o erro
                return Json(new { success = false, message = "Erro ao processar a solicitação. Detalhes: " + ex.Message });
            }
        }
        public IActionResult AtualizarAtendimento(int id, DateTime DtHorarioAgendamento, DateOnly DataAtendimento, TimeOnly Horario, int fk_Usuario, int fk_Servico)
        {
            try
            {
                // Chama o método do repositório para atualizar o atendimento
                var resultado = _agendamentoRepositorio.AtualizarAgendamento(id, DtHorarioAgendamento, DataAtendimento, Horario, fk_Usuario, fk_Servico);

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
                var resultado = _agendamentoRepositorio.ExcluirAgendamento(id);

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

            var agendamento = _agendamentoRepositorio.ConsultarAgendamento(data);

            if (agendamento != null)
            {
                return Json(agendamento);
            }
            else
            {
                return NotFound();
            }

        }
    };
}

