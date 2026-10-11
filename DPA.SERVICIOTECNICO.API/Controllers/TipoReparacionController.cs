using DPA.SERVICIOTECNICO.CORE.Core.Entities;
using DPA.SERVICIOTECNICO.CORE.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DPA.SERVICIOTECNICO.API.Controllers
{
    // Controlador CRUD con la estructura de la plantilla «API Controller with actions, using Entity Framework»:
    // trabaja con el DbContext inyectado directamente, sin repositorio.
    [Route("api/[controller]")]
    [ApiController]
    public class TipoReparacionController : ControllerBase
    {
        private readonly ServicioTecnicoDbContext _context;

        public TipoReparacionController(ServicioTecnicoDbContext context)
        {
            _context = context;
        }

        // GET: api/TipoReparacion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoReparacion>>> GetTipoReparacion()
        {
            return await _context.TipoReparacion.ToListAsync();
        }

        // GET: api/TipoReparacion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoReparacion>> GetTipoReparacion(int id)
        {
            var tipoReparacion = await _context.TipoReparacion.FindAsync(id);

            if (tipoReparacion == null)
            {
                return NotFound();
            }

            return tipoReparacion;
        }

        // PUT: api/TipoReparacion/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoReparacion(int id, TipoReparacion tipoReparacion)
        {
            if (id != tipoReparacion.Id)
            {
                return BadRequest();
            }

            _context.Entry(tipoReparacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoReparacionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TipoReparacion
        [HttpPost]
        public async Task<ActionResult<TipoReparacion>> PostTipoReparacion(TipoReparacion tipoReparacion)
        {
            _context.TipoReparacion.Add(tipoReparacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoReparacion", new { id = tipoReparacion.Id }, tipoReparacion);
        }

        // DELETE: api/TipoReparacion/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoReparacion(int id)
        {
            var tipoReparacion = await _context.TipoReparacion.FindAsync(id);
            if (tipoReparacion == null)
            {
                return NotFound();
            }

            _context.TipoReparacion.Remove(tipoReparacion);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // La clave foránea de OrdenReparacion impide eliminar un tipo que ya tiene órdenes asociadas.
                return Conflict("No se puede eliminar: el tipo de reparación tiene órdenes asociadas.");
            }

            return NoContent();
        }

        private bool TipoReparacionExists(int id)
        {
            return _context.TipoReparacion.Any(e => e.Id == id);
        }
    }
}
