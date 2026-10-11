using System.ComponentModel.DataAnnotations;

namespace DPA.SERVICIOTECNICO.CORE.Core.DTOs
{
    // Respuesta de la API: la orden con el equipo y el tipo de reparación en texto, sin exponer las entidades.
    public class OrdenReparacionListDTO
    {
        public int Id { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string DescripcionProblema { get; set; } = string.Empty;
        public decimal CostoEstimado { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int EquipoId { get; set; }
        public string Equipo { get; set; } = string.Empty;
        public int TipoReparacionId { get; set; }
        public string TipoReparacion { get; set; } = string.Empty;
    }

    // Datos para registrar una orden. [ApiController] valida los atributos antes de ejecutar la acción; las reglas
    // que requieren consultar la base (existencia del equipo y del tipo de reparación) las valida el servicio.
    public class OrdenReparacionCreateDTO
    {
        // Opcional: si se omite, el servicio asigna la fecha y hora actuales.
        public DateTime? FechaIngreso { get; set; }

        [Required(ErrorMessage = "La descripción del problema es obligatoria.")]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "La descripción debe tener entre 3 y 500 caracteres.")]
        public string DescripcionProblema { get; set; } = string.Empty;

        // Límites numéricos: un límite en texto se interpreta con la cultura del equipo y, con coma decimal,
        // «99999999.99» no es un número válido.
        [Range(0d, 99999999.99d, ErrorMessage = "El costo estimado no puede ser negativo.")]
        public decimal CostoEstimado { get; set; }

        // Opcional: si se omite, la orden se registra como «Pendiente».
        [StringLength(20)]
        public string? Estado { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Indique el equipo de la orden.")]
        public int EquipoId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Indique el tipo de reparación.")]
        public int TipoReparacionId { get; set; }
    }

    // Datos para actualizar una orden: los mismos campos más el Id, que debe coincidir con el de la ruta.
    public class OrdenReparacionUpdateDTO : OrdenReparacionCreateDTO
    {
        public int Id { get; set; }
    }
}
