using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Zona_Geek.Models;
using Zona_Geek.ORM;

namespace SiteAgendamento.Repositorio
{
    public class AtendimentoRepositorio
    {

        private BdZonaGeekContext _context;
        public AtendimentoRepositorio(BdZonaGeekContext context)
        {
            _context = context;
        }
        public List<ViewAtendimentoVM> ListarAtendimentos()
        {
            List<ViewAtendimentoVM> listAtendimentos = new List<ViewAtendimentoVM>();

            // Recuperando todos os atendimentos do DbSet
            var listTb = _context.ViewAtendimentos.ToList();

            // Convertendo os atendimentos de TbAtendimento para AtendimentoVM
            foreach (var item in listTb)
            {
                var atendimento = new ViewAtendimentoVM
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
        public bool InserirAtendimento( int Id, DateTime DtHorarioAgendamento, DateOnly DataAtendimento, TimeOnly Horario, int Fk_Usuario, int Fk_Servico)
        {
            try
            {
                // Criando uma instância do modelo AtendimentoVM
                Atendimento atendimento = new Atendimento
                {
                    Id = Id,
                    DtHorarioAgendamento = DtHorarioAgendamento,
                    DataAtendimento = DataAtendimento,
                    Horario = Horario,
                    FkUsuario = Fk_Usuario ,
                    FkServico= Fk_Servico 
                };

                // Supondo que _context.TbServicos seja o DbSet para a entidade de serviços no seu DbContext
                _context.Atendimentos.Add(atendimento);  // Adiciona o serviço ao contexto
                _context.SaveChanges();  // Salva as mudanças no banco de dados

                return true;  // Retorna true para indicar que a operação foi bem-sucedida
            }
            catch (Exception ex)
            {
                // Aqui você pode registrar o erro (ex.Message) ou tratá-lo conforme necessário
                return false;  // Retorna false em caso de falha
            }
        }       
        public bool AtualizarAtendimento(int id, DateTime DtHorarioAgendamento, DateOnly DataAtendimento,TimeOnly Horario, int Fk_Usuario,int Fk_Servico )
        {
            try
            {
                // Busca o serviço pelo ID
                var atendimento = _context.Atendimentos.FirstOrDefault(s => s.Id == id);
                if (atendimento != null)
                {
                    // Atualiza os dados do serviço
                    atendimento.DtHorarioAgendamento = DtHorarioAgendamento;
                    atendimento.DataAtendimento = DataAtendimento;
                    atendimento.Horario = Horario;

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
                Console.WriteLine($"Erro ao atualizar o atendimento com ID {id}: {ex.Message}");
                return false;
            }
        }
        public bool ExcluirAtendimento(int id)
        {
            try
            {
                // Busca o serviço pelo ID
                var atendimento = _context.Atendimentos.FirstOrDefault(s => s.Id == id);

                // Se o serviço não for encontrado, lança uma exceção personalizada
                if (atendimento == null)
                {
                    throw new KeyNotFoundException("Atendimento não encontrado.");
                }

                // Remove o serviço do banco de dados
                _context.Atendimentos.Remove(atendimento);
                _context.SaveChanges();  // Isso pode lançar uma exceção se houver dependências

                // Se tudo correr bem, retorna true indicando sucesso
                return true;

            }
            catch (Exception ex)
            {
                // Aqui tratamos qualquer erro inesperado e logamos para depuração
                Console.WriteLine($"Erro ao excluir o atendimento com ID {id}: {ex.Message}");

                // Relança a exceção para ser capturada pelo controlador
                throw new Exception($"Erro ao excluir o atendimento: {ex.Message}");
            }
        }

        public List<AtendimentoVM> ConsultarAgendamento(string datap)
        {
            DateOnly data = DateOnly.ParseExact(datap, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string dataFormatada = data.ToString("yyyy-MM-dd"); // Formato desejado: "yyyy-MM-dd"

            try
            {
                // Consulta ao banco de dados, convertendo para o tipo AtendimentoVM
                var ListarAtendimento = _context.Atendimentos
                    .Where(a => a.DataAtendimento == DateOnly.Parse(dataFormatada))
                    .Select(a => new AtendimentoVM
                    {
                        // Mapear as propriedades de TbAtendimento para AtendimentoVM
                        // Suponha que TbAtendimento tenha as propriedades Id, DataAtendimento, e outras:
                        Id = a.Id,
                        DtHorarioAgendamento = a.DtHorarioAgendamento,
                        DataAtendimento = DateOnly.Parse(dataFormatada),
                        Horario = a.Horario,
                        Fk_Usuario = a.Fk_Usuario,
                        Fk_Servico = a.Fk_Servico
                    })
                    .ToList(); // Converte para uma lista

                return ListarAtendimento;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao consultar agendamentos: {ex.Message}");
                return new List<AtendimentoVM>(); // Retorna uma lista vazia em caso de erro
            }
        }

    }
}
