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
        public bool InserirAgendamento( int Id, DateTime DtHorarioAgendamento, DateOnly DataAtendimento, TimeOnly Horario, int Fk_Usuario, int Fk_Servico)
        {
            try
            {
                // Criando uma instância do modelo AtendimentoVM
                Agendamento agendamento = new Agendamento
                {
                    Id = Id,
                    DtHorarioAgendamento = DtHorarioAgendamento,
                    DataAtendimento = DataAtendimento,
                    Horario = Horario,
                    FkUsuario = Fk_Usuario ,
                    FkServico= Fk_Servico 
                };

                // Supondo que _context.TbServicos seja o DbSet para a entidade de serviços no seu DbContext
                _context.Agendamentos.Add(agendamento);  // Adiciona o serviço ao contexto
                _context.SaveChanges();  // Salva as mudanças no banco de dados

                return true;  // Retorna true para indicar que a operação foi bem-sucedida
            }
            catch (Exception ex)
            {
                // Aqui você pode registrar o erro (ex.Message) ou tratá-lo conforme necessário
                return false;  // Retorna false em caso de falha
            }
        }       
        public bool AtualizarAgendamento(int id, DateTime DtHorarioAgendamento, DateOnly DataAtendimento,TimeOnly Horario, int Fk_Usuario,int Fk_Servico )
        {
            try
            {
                // Busca o serviço pelo ID
                var agendamento = _context.Agendamentos.FirstOrDefault(s => s.Id == id);
                if (agendamento != null)
                {
                    // Atualiza os dados do serviço
                    agendamento.DtHorarioAgendamento = DtHorarioAgendamento;
                    agendamento.DataAtendimento = DataAtendimento;
                    agendamento.Horario = Horario;

                    // Salva as mudanças no banco de dados
                    _context.SaveChanges();

                    return true;  // Retorna verdadeiro se a atualização for bem-sucedida
                }
                else
                {
                    return false;  // Retorna falso se o serviço não for encontrado
                }
            }
            catch (Exception ex)
            {
                // Em caso de erro, loga a exceção (opcional)
                Console.WriteLine($"Erro ao atualizar o agendamento com ID {id}: {ex.Message}");
                return false;
            }
        }
        public bool ExcluirAgendamento(int id)
        {
            try
            {
                // Busca o serviço pelo ID
                var agendamento = _context.Agendamentos.FirstOrDefault(s => s.Id == id);

                // Se o serviço não for encontrado, lança uma exceção personalizada
                if (agendamento == null)
                {
                    throw new KeyNotFoundException("Agendamento não encontrado.");
                }

                // Remove o serviço do banco de dados
                _context.Agendamentos.Remove(agendamento);
                _context.SaveChanges();  // Isso pode lançar uma exceção se houver dependências

                // Se tudo correr bem, retorna true indicando sucesso
                return true;

            }
            catch (Exception ex)
            {
                // Aqui tratamos qualquer erro inesperado e logamos para depuração
                Console.WriteLine($"Erro ao excluir o agendamento com ID {id}: {ex.Message}");

                // Relança a exceção para ser capturada pelo controlador
                throw new Exception($"Erro ao excluir o agendamento: {ex.Message}");
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

    }
}
