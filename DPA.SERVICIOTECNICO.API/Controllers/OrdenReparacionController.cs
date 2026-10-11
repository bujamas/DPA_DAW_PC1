using DPA.SERVICIOTECNICO.CORE.Core.DTOs;
using DPA.SERVICIOTECNICO.CORE.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DPA.SERVICIOTECNICO.API.Controllers
{
    // Depende únicamente de IOrdenReparacionService. Las interfaces y los DTO están en Core; el repositorio y el
    // servicio, en Infrastructure.
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenReparacionController : ControllerBase
    {
        private readonly IOrdenReparacionService _ordenReparacionService;

        public OrdenReparacionController(IOrdenReparacionService ordenReparacionService)
        {
            _ordenReparacionService = ordenReparacionService;
        }

        // GET: api/OrdenReparacion
        [HttpGet]
        [ProducesResponseType<IEnumerable<OrdenReparacionListDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrdenesReparacion()
        {
            var ordenes = await _ordenReparacionService.GetOrdenesReparacion();
            return Ok(ordenes);
        }

        // GET: api/OrdenReparacion/5
        [HttpGet("{id}")]
        [ProducesResponseType<OrdenReparacionListDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrdenReparacion(int id)
        {
            var orden = await _ordenReparacionService.GetOrdenReparacionById(id);
            if (orden == null)
            {
                return NotFound();
            }
            return Ok(orden);
        }

        // POST: api/OrdenReparacion
        [HttpPost]
        [ProducesResponseType<OrdenReparacionListDTO>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrdenReparacion([FromBody] OrdenReparacionCreateDTO ordenDTO)
        {
            var resultado = await _ordenReparacionService.CreateOrdenReparacion(ordenDTO);
            if (!resultado.Exito)
            {
                return BadRequest(new { mensaje = resultado.Error });
            }
            return CreatedAtAction(nameof(GetOrdenReparacion), new { id = resultado.Datos!.Id }, resultado.Datos);
        }

        // PUT: api/OrdenReparacion/5
        [HttpPut("{id}")]
        [ProducesResponseType<OrdenReparacionListDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrdenReparacion(int id, [FromBody] OrdenReparacionUpdateDTO ordenDTO)
        {
            if (id != ordenDTO.Id)
            {
                return BadRequest(new { mensaje = "El Id de la ruta no coincide con el de la orden." });
            }

            var resultado = await _ordenReparacionService.UpdateOrdenReparacion(ordenDTO);
            if (resultado.NoEncontrado)
            {
                return NotFound();
            }
            if (!resultado.Exito)
            {
                return BadRequest(new { mensaje = resultado.Error });
            }
            return Ok(resultado.Datos);
        }

        // DELETE: api/OrdenReparacion/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrdenReparacion(int id)
        {
            var eliminada = await _ordenReparacionService.DeleteOrdenReparacion(id);
            if (!eliminada)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
