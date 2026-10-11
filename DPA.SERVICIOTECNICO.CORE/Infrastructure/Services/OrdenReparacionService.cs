using DPA.SERVICIOTECNICO.CORE.Core.DTOs;
using DPA.SERVICIOTECNICO.CORE.Core.Entities;
using DPA.SERVICIOTECNICO.CORE.Core.Interfaces;

namespace DPA.SERVICIOTECNICO.CORE.Infrastructure.Services
{
    // Capa intermedia entre el controlador y el repositorio: convierte entidades en DTO y valida las reglas que
    // requieren consultar la base antes de guardar.
    public class OrdenReparacionService : IOrdenReparacionService
    {
        // Valores admitidos por la restricción CK_OrdenReparacion_Estado.
        private static readonly string[] EstadosValidos = ["Pendiente", "En proceso", "Reparado", "Entregado", "Cancelado"];

        private readonly IOrdenReparacionRepository _ordenReparacionRepository;

        public OrdenReparacionService(IOrdenReparacionRepository ordenReparacionRepository)
        {
            _ordenReparacionRepository = ordenReparacionRepository;
        }

        public async Task<IEnumerable<OrdenReparacionListDTO>> GetOrdenesReparacion()
        {
            var ordenes = await _ordenReparacionRepository.GetOrdenesReparacion();
            return ordenes.Select(Mapear).ToList();
        }

        public async Task<OrdenReparacionListDTO?> GetOrdenReparacionById(int id)
        {
            var orden = await _ordenReparacionRepository.GetOrdenReparacionById(id);
            return orden == null ? null : Mapear(orden);
        }

        public async Task<ResultadoOperacion<OrdenReparacionListDTO>> CreateOrdenReparacion(OrdenReparacionCreateDTO ordenDTO)
        {
            var error = await Validar(ordenDTO);
            if (error != null)
            {
                return ResultadoOperacion<OrdenReparacionListDTO>.Invalido(error);
            }

            var orden = new OrdenReparacion
            {
                FechaIngreso = ordenDTO.FechaIngreso ?? AhoraEnSegundos(),
                DescripcionProblema = ordenDTO.DescripcionProblema.Trim(),
                CostoEstimado = ordenDTO.CostoEstimado,
                Estado = NormalizarEstado(ordenDTO.Estado),
                EquipoId = ordenDTO.EquipoId,
                TipoReparacionId = ordenDTO.TipoReparacionId
            };

            if (!await _ordenReparacionRepository.CreateOrdenReparacion(orden))
            {
                return ResultadoOperacion<OrdenReparacionListDTO>.Invalido("No se pudo registrar la orden de reparación.");
            }

            // Se consulta de nuevo para devolver el equipo y el tipo de reparación con sus nombres.
            var creada = await _ordenReparacionRepository.GetOrdenReparacionById(orden.Id);
            return ResultadoOperacion<OrdenReparacionListDTO>.Correcto(Mapear(creada!));
        }

        public async Task<ResultadoOperacion<OrdenReparacionListDTO>> UpdateOrdenReparacion(OrdenReparacionUpdateDTO ordenDTO)
        {
            var orden = await _ordenReparacionRepository.GetOrdenReparacionById(ordenDTO.Id);
            if (orden == null)
            {
                return ResultadoOperacion<OrdenReparacionListDTO>.SinRegistro();
            }

            var error = await Validar(ordenDTO);
            if (error != null)
            {
                return ResultadoOperacion<OrdenReparacionListDTO>.Invalido(error);
            }

            if (ordenDTO.FechaIngreso.HasValue)
            {
                orden.FechaIngreso = ordenDTO.FechaIngreso.Value;
            }
            orden.DescripcionProblema = ordenDTO.DescripcionProblema.Trim();
            orden.CostoEstimado = ordenDTO.CostoEstimado;
            orden.Estado = NormalizarEstado(ordenDTO.Estado);
            orden.EquipoId = ordenDTO.EquipoId;
            orden.TipoReparacionId = ordenDTO.TipoReparacionId;

            await _ordenReparacionRepository.UpdateOrdenReparacion(orden);

            var actualizada = await _ordenReparacionRepository.GetOrdenReparacionById(orden.Id);
            return ResultadoOperacion<OrdenReparacionListDTO>.Correcto(Mapear(actualizada!));
        }

        public async Task<bool> DeleteOrdenReparacion(int id)
        {
            return await _ordenReparacionRepository.DeleteOrdenReparacion(id);
        }

        // La columna es datetime2(0): se descartan las fracciones de segundo para que el POST devuelva la misma fecha
        // que luego devolverá el GET.
        private static DateTime AhoraEnSegundos()
        {
            var ahora = DateTime.Now;
            return new DateTime(ahora.Year, ahora.Month, ahora.Day, ahora.Hour, ahora.Minute, ahora.Second, DateTimeKind.Unspecified);
        }

        // Reglas que no pueden validarse solo con los atributos del DTO.
        private async Task<string?> Validar(OrdenReparacionCreateDTO ordenDTO)
        {
            if (!string.IsNullOrWhiteSpace(ordenDTO.Estado) && !EstadosValidos.Contains(ordenDTO.Estado.Trim(), StringComparer.OrdinalIgnoreCase))
            {
                return $"El estado debe ser uno de estos: {string.Join(", ", EstadosValidos)}.";
            }
            if (!await _ordenReparacionRepository.ExisteEquipo(ordenDTO.EquipoId))
            {
                return $"No existe el equipo con Id {ordenDTO.EquipoId}.";
            }
            if (!await _ordenReparacionRepository.ExisteTipoReparacion(ordenDTO.TipoReparacionId))
            {
                return $"No existe el tipo de reparación con Id {ordenDTO.TipoReparacionId}.";
            }
            return null;
        }

        // Normaliza el estado a la escritura de la lista («en proceso» se guarda como «En proceso»).
        private static string NormalizarEstado(string? estado)
        {
            if (string.IsNullOrWhiteSpace(estado))
            {
                return "Pendiente";
            }
            return EstadosValidos.First(e => string.Equals(e, estado.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        private static OrdenReparacionListDTO Mapear(OrdenReparacion orden)
        {
            return new OrdenReparacionListDTO
            {
                Id = orden.Id,
                FechaIngreso = orden.FechaIngreso,
                DescripcionProblema = orden.DescripcionProblema,
                CostoEstimado = orden.CostoEstimado,
                Estado = orden.Estado,
                EquipoId = orden.EquipoId,
                Equipo = orden.Equipo == null ? string.Empty : $"{orden.Equipo.Marca} {orden.Equipo.Modelo} ({orden.Equipo.NumeroSerie})",
                TipoReparacionId = orden.TipoReparacionId,
                TipoReparacion = orden.TipoReparacion?.Nombre ?? string.Empty
            };
        }
    }
}
