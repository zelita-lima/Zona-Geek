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
            var listaHorario = new List<SelectListItem>
             {
                 new SelectListItem { Value = "8", Text = "08:00:00" },
                 new SelectListItem { Value = "10", Text = "10:00:00" },
                 new SelectListItem { Value = "13", Text = "13:00:00" },
                 new SelectListItem { Value = "15", Text = "15:00:00" },
                 new SelectListItem { Value = "17", Text = "17:00:00" },
                 new SelectListItem { Value = "19", Text = "19:00:00" }
             };

            ViewBag.lstHorarios = listaHorario;
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
        public IActionResult AlterarAgendamento(int id, string data, int servico, TimeOnly horario)
        {

            var rs = _agendamentoRepositorio.AlterarAgendamento(id, data, servico, horario);
            if (rs)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        public IActionResult ExcluirAgendamento(int id)
        {

            var rs = _agendamentoRepositorio.ExcluirAgendamento(id);
            return Json(new { success = rs });

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

