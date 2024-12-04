using System;
using System.Collections.Generic;

namespace Zona_Geek.ORM;

public partial class Servico
{
    public int Id { get; set; }

    public string TipoServico { get; set; } = null!;

    public decimal Valor { get; set; }

    public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
}
