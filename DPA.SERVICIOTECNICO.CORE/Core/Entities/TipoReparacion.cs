using System;
using System.Collections.Generic;

namespace DPA.SERVICIOTECNICO.CORE.Core.Entities;

public partial class TipoReparacion
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal PrecioBase { get; set; }

    public virtual ICollection<OrdenReparacion> OrdenReparacion { get; set; } = new List<OrdenReparacion>();
}
