using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Zona_Geek.Models;
using Zona_Geek.ORM;

namespace SiteAgendamento.Repositorio
{
    public class AgendamentoRepositorio
    {
        private BdZonaGeekContext _context;
        public AgendamentoRepositorio(BdZonaGeekContext context)
        {
            _context = context;
        }
        public List<ViewAgendamentoVM> ListarAgendamentos()
        {
            List<ViewAgendamentoVM> listAgendamentos = new List<ViewAgendamentoVM>();

            // Recuperando todos os atendimentos do DbSet
            var listTb = _context.ViewAgendamentos.ToList();

            // Convertendo os atendimentos de TbAtendimento para AtendimentoVM
            foreach (var item in listTb)
            {
                var agendamento = new ViewAgendamentoVM
                {
                    Id = item.Id,
                    DtHorarioAgendamento = item.DtHorarioAgendamento,
                    DataAtendimento = item.DataAtendimento,
                    Horario = item.Horario,
                    TipoServico = item.TipoServico,
                    Valor = item.Valor,
                    Nome = item.Nome,
                    Email = item.Email,
                    Telefone = item.Telefone,

                };

                listAgendamentos.Add(agendamento);
            }

            return listAgendamentos;
        }
        // Método para inserir um novo agendamento
        public bool InserirAgendamento(DateTime dtHoraAgendamento, DateOnly dataAtendimento, TimeOnly horario, int fkUsuarioId, int fkServicoId)
        {
            try
            {
                // Criando uma instância do modelo AtendimentoVM
                var atendimento = new Agendamento
                {
                    DtHorarioAgendamento = dtHoraAgendamento,
                    DataAtendimento = dataAtendimento,
                    Horario = horario,
                    FkUsuario = fkUsuarioId,
                    FkServico = fkServicoId
                };

                // Adicionando o atendimento ao contexto
                _context.Agendamentos.Add(atendimento);
                _context.SaveChanges(); // Persistindo as mudanças no banco de dados

                return true; // Retorna true indicando sucesso
            }
            catch (Exception ex)
            {
                // Em caso de erro, pode-se logar a exceção (ex.Message)
                return false; // Retorna false em caso de erro
            }
        }
        public bool AlterarAgendamento(int id, string data, int servico, TimeOnly horario)
        {
            try
            {
                Agendamento agt = _context.Agendamentos.Find(id);
                DateOnly dtHoraAgendamento;
                if (agt != null)
                {
                    agt.Id = id;
                    if (data != null)
                    {
                        if (DateOnly.TryParse(data, out dtHoraAgendamento))
                        {
                            agt.DataAtendimento = dtHoraAgendamento;
                        }
                    }

                    // Corrigido a verificação do tipo TimeOnly
                    if (horario != TimeOnly.MinValue)  // Verificando se o horário não é o valor padrão
                    {
                        agt.Horario = horario;
                    }

                    agt.FkServico = servico;
                    _context.SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool ExcluirAgendamento(int id)
        {
            try
            {


                var agt = _context.Agendamentos.Where(a => a.Id == id).FirstOrDefault();
                if (agt != null)
                {
                    _context.Agendamentos.Remove(agt);

                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception)
            {

                return false;
            }
        }
        public List<AgendamentoVM> ConsultarAgendamento(string datap)
        {
            DateOnly data = DateOnly.ParseExact(datap, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            string dataFormatada = data.ToString("yyyy-MM-dd"); // Formato desejado: "yyyy-MM-dd"
            Console.WriteLine(dataFormatada);

            try
            {
                // Consulta ao banco de dados, convertendo para o tipo AtendimentoVM
                var ListaAgendamento = _context.Agendamentos
                    .Where(a => a.DataAtendimento == DateOnly.Parse(dataFormatada))
                    .Select(a => new AgendamentoVM
                    {
                        // Mapear as propriedades de TbAtendimento para AtendimentoVM
                        // Suponha que TbAtendimento tenha as propriedades Id, DataAtendimento, e outras:
                        Id = a.Id,
                        DtHorarioAgendamento = a.DtHorarioAgendamento,
                        DataAtendimento = DateOnly.Parse(dataFormatada),
                        Horario = a.Horario,
                        Fk_Usuario = a.FkUsuario,
                        Fk_Servico = a.FkServico
                    })
                    .ToList(); // Converte para uma lista

                return ListaAgendamento;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao consultar agendamentos: {ex.Message}");
                return new List<AgendamentoVM>(); // Retorna uma lista vazia em caso de erro
            }
        }
        public List<UsuarioVM> ListarNomesAgendamentos()
        {
            // Lista para armazenar os usuários com apenas Id e Nome
            List<UsuarioVM> listFun = new List<UsuarioVM>();

            // Obter apenas os campos Id e Nome da tabela TbUsuarios
            var listTb = _context.Usuarios
                                 .Select(u => new UsuarioVM
                                 {
                                     Id = u.Id,
                                     Nome = u.Nome
                                 })
                                 .ToList();

            // Retorna a lista já com os campos filtrados
            return listTb;
        }
        public List<ViewAgendamentoVM> ListarAgendamentosCliente()
        {
            // Obtendo o ID do usuário a partir da variável de ambiente
            string nome = Environment.GetEnvironmentVariable("USUARIO_NOME");

            List<ViewAgendamentoVM> listAtendimentos = new List<ViewAgendamentoVM>();

            // Recuperando todos os agendamentos que correspondem ao ID do usuário
            var listTb = _context.ViewAgendamentos.Where(x => x.Nome == nome).ToList();

            // Convertendo cada agendamento para ViewAgendamentoVM
            foreach (var item in listTb)
            {
                var atendimento = new ViewAgendamentoVM
                {
                    Id = item.Id,
                    DtHorarioAgendamento = item.DtHorarioAgendamento,
                    DataAtendimento = item.DataAtendimento,
                    Horario = item.Horario,
                    TipoServico = item.TipoServico,
                    Valor = item.Valor,
                    Nome = item.Nome,
                    Email = item.Email,
                    Telefone = item.Telefone,
                };

                listAtendimentos.Add(atendimento);
            }

            return listAtendimentos;
        }

    }
}
