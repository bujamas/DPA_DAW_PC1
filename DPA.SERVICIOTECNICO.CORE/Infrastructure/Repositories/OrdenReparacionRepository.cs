using DPA.SERVICIOTECNICO.CORE.Core.Entities;
using DPA.SERVICIOTECNICO.CORE.Core.Interfaces;
using DPA.SERVICIOTECNICO.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DPA.SERVICIOTECNICO.CORE.Infrastructure.Repositories
{
    public class OrdenReparacionRepository : IOrdenReparacionRepository
    {
        private readonly ServicioTecnicoDbContext _context;

        public OrdenReparacionRepository(ServicioTecnicoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrdenReparacion>> GetOrdenesReparacion()
        {
            // Consulta de solo lectura: AsNoTracking evita el seguimiento de cambios.
            return await _context.OrdenReparacion
                .AsNoTracking()
                .Include(o => o.Equipo)
                .Include(o => o.TipoReparacion)
                .OrderBy(o => o.Id)
                .ToListAsync();
        }

        public async Task<OrdenReparacion?> GetOrdenReparacionById(int id)
        {
            // Con seguimiento de cambios, porque el servicio actualiza esta misma entidad.
            return await _context.OrdenReparacion
                .Include(o => o.Equipo)
                .Include(o => o.TipoReparacion)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<bool> CreateOrdenReparacion(OrdenReparacion ordenReparacion)
        {
            await _context.OrdenReparacion.AddAsync(ordenReparacion);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateOrdenReparacion(OrdenReparacion ordenReparacion)
        {
            _context.OrdenReparacion.Update(ordenReparacion);
            // SaveChanges devuelve 0 si los datos no cambiaron; no se considera un error.
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOrdenReparacion(int id)
        {
            var ordenReparacion = await _context.OrdenReparacion.FindAsync(id);
            if (ordenReparacion == null)
            {
                return false;
            }

            _context.OrdenReparacion.Remove(ordenReparacion);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> ExisteEquipo(int equipoId)
        {
            return await _context.Equipo.AnyAsync(e => e.Id == equipoId);
        }

        public async Task<bool> ExisteTipoReparacion(int tipoReparacionId)
        {
            return await _context.TipoReparacion.AnyAsync(t => t.Id == tipoReparacionId);
        }
    }
}
