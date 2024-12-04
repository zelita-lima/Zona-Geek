using System;
using System.Collections.Generic;

namespace Zona_Geek.ORM;

public partial class Agendamento
{
    public int Id { get; set; }

    public DateTime DtHorarioAgendamento { get; set; }

    public DateOnly DataAtendimento { get; set; }

    public TimeOnly Horario { get; set; }

    public int FkUsuario { get; set; }

    public int FkServico { get; set; }

    public virtual Servico FkServicoNavigation { get; set; } = null!;

    public virtual Usuario FkUsuarioNavigation { get; set; } = null!;
}
