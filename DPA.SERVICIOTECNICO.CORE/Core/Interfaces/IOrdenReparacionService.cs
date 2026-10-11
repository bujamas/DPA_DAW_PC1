using DPA.SERVICIOTECNICO.CORE.Core.DTOs;

namespace DPA.SERVICIOTECNICO.CORE.Core.Interfaces
{
    // Reglas de negocio de las órdenes de reparación, expuestas mediante DTO. La implementación está en
    // Infrastructure/Services.
    public interface IOrdenReparacionService
    {
        Task<IEnumerable<OrdenReparacionListDTO>> GetOrdenesReparacion();
        Task<OrdenReparacionListDTO?> GetOrdenReparacionById(int id);
        Task<ResultadoOperacion<OrdenReparacionListDTO>> CreateOrdenReparacion(OrdenReparacionCreateDTO ordenDTO);
        Task<ResultadoOperacion<OrdenReparacionListDTO>> UpdateOrdenReparacion(OrdenReparacionUpdateDTO ordenDTO);
        Task<bool> DeleteOrdenReparacion(int id);
    }
}
