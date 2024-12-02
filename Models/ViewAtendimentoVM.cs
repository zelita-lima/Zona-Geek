﻿namespace Zona_Geek.Models
{
    public class ViewAtendimentoVM
    {
        public int Id { get; set; }

        public DateTime DtHorarioAgendamento { get; set; }

        public DateOnly DataAtendimento { get; set; }

        public TimeOnly Horario { get; set; }

        public string TipoServico { get; set; } = null!;

        public decimal Valor { get; set; }

        public string Nome { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Telefone { get; set; } = null!;
    }

}

