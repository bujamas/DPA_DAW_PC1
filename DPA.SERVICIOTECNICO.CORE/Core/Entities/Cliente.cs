using System;
using System.Collections.Generic;

namespace DPA.SERVICIOTECNICO.CORE.Core.Entities;

public partial class Cliente
{
    public int Id { get; set; }

    public string Paterno { get; set; } = null!;

    public string? Materno { get; set; }

    public string Nombres { get; set; } = null!;

    public string? Correo { get; set; }

    public string? Telefono { get; set; }

    public virtual ICollection<Equipo> Equipo { get; set; } = new List<Equipo>();
}
