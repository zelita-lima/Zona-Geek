namespace Zona_Geek.Models
{
    public class AtendimentoVM
    {
        public int Id { get; set; }

        public string DtHorarioAgendamento { get; set; } = null!;

        public string DataAtendimento { get; set; } = null!;

        public string Horario { get; set; } = null!;

        public int Fk_Usuario { get; set; }

        public int Fk_Servico { get; set; }
    }
}
