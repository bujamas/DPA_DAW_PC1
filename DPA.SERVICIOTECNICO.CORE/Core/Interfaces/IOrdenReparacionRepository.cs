using DPA.SERVICIOTECNICO.CORE.Core.Entities;

namespace DPA.SERVICIOTECNICO.CORE.Core.Interfaces
{
    // Acceso a datos de las órdenes de reparación. La implementación está en Infrastructure/Repositories.
    public interface IOrdenReparacionRepository
    {
        Task<IEnumerable<OrdenReparacion>> GetOrdenesReparacion();
        Task<OrdenReparacion?> GetOrdenReparacionById(int id);
        Task<bool> CreateOrdenReparacion(OrdenReparacion ordenReparacion);
        Task<bool> UpdateOrdenReparacion(OrdenReparacion ordenReparacion);
        Task<bool> DeleteOrdenReparacion(int id);
        Task<bool> ExisteEquipo(int equipoId);
        Task<bool> ExisteTipoReparacion(int tipoReparacionId);
    }
}
