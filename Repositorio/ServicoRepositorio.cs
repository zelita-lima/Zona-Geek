﻿using Microsoft.EntityFrameworkCore;
using Zona_Geek.Models;
using Zona_Geek.ORM;

namespace SiteAgendamento.Repositorio
{
    public class ServicoRepositorio
    {

        private BdZonaGeekContext _context;
        public ServicoRepositorio(BdZonaGeekContext context)
        {
            _context = context;
        }
        public bool InserirServico(string tipoServico, decimal valor)
        {
            try
            {
                // Criando uma instância do modelo ServicoVM
                Servico servico = new Servico
                {
                    TipoServico = tipoServico,
                    Valor = valor
                };

                // Supondo que _context.TbServicos seja o DbSet para a entidade de serviços no seu DbContext
                _context.Servicos.Add(servico);  // Adiciona o serviço ao contexto
                _context.SaveChanges();  // Salva as mudanças no banco de dados

                return true;  // Retorna true para indicar que a operação foi bem-sucedida
            }
            catch (Exception ex)
            {
                // Aqui você pode registrar o erro (ex.Message) ou tratá-lo conforme necessário
                return false;  // Retorna false em caso de falha
            }
        }
        public List<ServicoVM> ListarServicos()
        {
            List<ServicoVM> listServicos = new List<ServicoVM>();

            // Recuperando todos os serviços do DbSet
            var listTb = _context.Servicos.ToList();

            foreach (var item in listTb)
            {
                var servico = new ServicoVM
                {
                    id = item.Id,
                    TipoServico = item.TipoServico,
                    Valor = item.Valor
                };

                listServicos.Add(servico);
            }

            return listServicos;
        }
        public bool AtualizarServico(int id, string tipoServico, decimal valor)
        {
            try
            {
                // Busca o serviço pelo ID
                var servico = _context.Servicos.FirstOrDefault(s => s.Id == id);
                if (servico != null)
                {
                    // Atualiza os dados do serviço
                    servico.TipoServico = tipoServico;
                    servico.Valor = valor;

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
                Console.WriteLine($"Erro ao atualizar o serviço com ID {id}: {ex.Message}");
                return false;
            }
        }
        public bool ExcluirServico(int id)
        {
            try
            {
                // Busca o serviço pelo ID
                var servico = _context.Servicos.FirstOrDefault(s => s.Id == id);

                // Se o serviço não for encontrado, lança uma exceção personalizada
                if (servico == null)
                {
                    throw new KeyNotFoundException("Serviço não encontrado.");
                }

                // Remove o serviço do banco de dados
                _context.Servicos.Remove(servico);
                _context.SaveChanges();  // Isso pode lançar uma exceção se houver dependências

                // Se tudo correr bem, retorna true indicando sucesso
                return true;

            }
            catch (Exception ex)
            {
                // Aqui tratamos qualquer erro inesperado e logamos para depuração
                Console.WriteLine($"Erro ao excluir o serviço com ID {id}: {ex.Message}");

                // Relança a exceção para ser capturada pelo controlador
                throw new Exception($"Erro ao excluir o serviço: {ex.Message}");
            }
        }

    }
}
