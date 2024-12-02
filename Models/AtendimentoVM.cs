namespace Zona_Geek.Models
{
    public class AtendimentoVM
    {
        public int Id { get; set; }

        public DateTime DtHorarioAgendamento { get; set; } 

        public DateOnly DataAtendimento { get; set; } 

        public TimeOnly Horario { get; set; } 

        public int Fk_Usuario { get; set; }

        public int Fk_Servico { get; set; }
    }
}
