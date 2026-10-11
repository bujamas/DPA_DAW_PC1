# DPA_DAW_PC1

Práctica Calificada 01 del curso Desarrollo de Ambiente Web, Universidad ESAN, 2026-2.

Alumno: 24100547

Web API en ASP.NET Core 10 con enfoque Database First para la gestión de un servicio técnico de equipos
electrónicos: tipos de reparación, clientes, equipos y órdenes de reparación, sobre SQL Server y Entity Framework
Core 10.

## Contenido por pregunta

| Pregunta | Tema | Ubicación |
|---|---|---|
| 1 | Diseño de la base de datos | [ServicioTecnicoDB.sql](BaseDeDatos/ServicioTecnicoDB.sql) y [diagrama entidad-relación](BaseDeDatos/DiagramaER.md) |
| 2 | Estructura del proyecto | [DPA.SERVICIOTECNICO.slnx](DPA.SERVICIOTECNICO.slnx), con la Web API `DPA.SERVICIOTECNICO.API` y la biblioteca de clases `DPA.SERVICIOTECNICO.CORE` |
| 3 | Scaffolding con Entity Framework Core | [Core/Entities](DPA.SERVICIOTECNICO.CORE/Core/Entities) e [Infrastructure/Data](DPA.SERVICIOTECNICO.CORE/Infrastructure/Data) |
| 4 | Controlador autogenerado | [TipoReparacionController.cs](DPA.SERVICIOTECNICO.API/Controllers/TipoReparacionController.cs) |
| 5 | Controlador con Repository e interfaces | [OrdenReparacionController.cs](DPA.SERVICIOTECNICO.API/Controllers/OrdenReparacionController.cs), con su servicio y su repositorio en la biblioteca de clases |
| 6 | Pruebas con Postman | [Pregunta6](DPA.SERVICIOTECNICO.API/Pregunta6) |
| 7 | Buenas prácticas y reflexión | [Pregunta7](DPA.SERVICIOTECNICO.API/Pregunta7) |

## Estructura de la solución

```
DPA_DAW_PC1
├── BaseDeDatos
│   ├── ServicioTecnicoDB.sql                    Script de creación de la base de datos
│   └── DiagramaER.md                            Diagrama entidad-relación en Mermaid
├── DPA.SERVICIOTECNICO.slnx
├── DPA.SERVICIOTECNICO.API                      Web API (capa de presentación)
│   ├── Controllers
│   │   ├── TipoReparacionController.cs          DbContext inyectado, sin repositorio
│   │   └── OrdenReparacionController.cs         Servicio inyectado mediante su interfaz
│   ├── Pregunta6                                Colección de Postman y resultados
│   ├── Pregunta7                                Respuestas en Word
│   ├── Program.cs                               Registro del DbContext, el repositorio y el servicio
│   └── appsettings.json                         Cadena de conexión DevConnection
└── DPA.SERVICIOTECNICO.CORE                     Biblioteca de clases
    ├── Core
    │   ├── DTOs                                 OrdenReparacionDTO y ResultadoOperacion
    │   ├── Entities                             Entidades generadas por el scaffolding
    │   └── Interfaces                           IOrdenReparacionRepository e IOrdenReparacionService
    └── Infrastructure
        ├── Data                                 ServicioTecnicoDbContext
        ├── Repositories                         OrdenReparacionRepository
        └── Services                             OrdenReparacionService
```

`Core` contiene las interfaces y los DTO; `Infrastructure`, la implementación de las interfaces y la lógica de
negocio.

## Base de datos

- Tablas `TipoReparacion`, `Cliente`, `Equipo` (con `ClienteId`) y `OrdenReparacion` (con `EquipoId` y
  `TipoReparacionId`).
- Claves primarias `IDENTITY(1,1)`, generadas por SQL Server.
- Claves foráneas con nombre y con un índice cada una.
- Restricciones `CHECK`: precio y costo no negativos, año entre 1990 y 2100, y estado entre Pendiente, En proceso,
  Reparado, Entregado y Cancelado.
- Número de serie único por equipo.
- Script idempotente, con datos de ejemplo.

Ejecución del script con sqlcmd (servidor local en el puerto 14330):

```
sqlcmd -S localhost,14330 -E -C -f 65001 -i BaseDeDatos\ServicioTecnicoDB.sql
```

## Scaffolding

Desde la Consola del Administrador de Paquetes de Visual Studio, con `DPA.SERVICIOTECNICO.CORE` como proyecto
predeterminado:

```
Scaffold-DbContext "Server=localhost,14330;Database=ServicioTecnicoDB;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Core/Entities -ContextDir Infrastructure/Data -Context ServicioTecnicoDbContext -Namespace DPA.SERVICIOTECNICO.CORE.Core.Entities -ContextNamespace DPA.SERVICIOTECNICO.CORE.Infrastructure.Data -NoOnConfiguring -NoPluralize -Project DPA.SERVICIOTECNICO.CORE -StartupProject DPA.SERVICIOTECNICO.CORE
```

Comando equivalente de la CLI de Entity Framework Core (`dotnet-ef` 10.0.12), utilizado en esta solución:

```
dotnet ef dbcontext scaffold "Server=localhost,14330;Database=ServicioTecnicoDB;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --project DPA.SERVICIOTECNICO.CORE --startup-project DPA.SERVICIOTECNICO.CORE --output-dir Core/Entities --context-dir Infrastructure/Data --context ServicioTecnicoDbContext --namespace DPA.SERVICIOTECNICO.CORE.Core.Entities --context-namespace DPA.SERVICIOTECNICO.CORE.Infrastructure.Data --no-onconfiguring --no-pluralize
```

## Ejecución

1. Crear la base de datos con el script de la Pregunta 1.
2. Si se usa otro servidor de SQL Server, actualizar `DevConnection` en `DPA.SERVICIOTECNICO.API/appsettings.json`.
3. Desde la carpeta de la solución, ejecutar `dotnet run --project DPA.SERVICIOTECNICO.API --launch-profile http`.
   La API queda disponible en http://localhost:5181.

Requisitos: SDK de .NET 10 y SQL Server.

## Endpoints

| Método | Ruta | Respuestas |
|---|---|---|
| GET | `/api/TipoReparacion` | 200 con la lista |
| GET | `/api/TipoReparacion/{id}` | 200 o 404 |
| POST | `/api/TipoReparacion` | 201 con el registro creado |
| PUT | `/api/TipoReparacion/{id}` | 204; 400 si el Id no coincide con la ruta; 404 |
| DELETE | `/api/TipoReparacion/{id}` | 204; 404; 409 si el tipo tiene órdenes asociadas |
| GET | `/api/OrdenReparacion` | 200 con la lista, con el equipo y el tipo de reparación en texto |
| GET | `/api/OrdenReparacion/{id}` | 200 o 404 |
| POST | `/api/OrdenReparacion` | 201 con la orden creada; 400 si los datos no son válidos |
| PUT | `/api/OrdenReparacion/{id}` | 200 con la orden actualizada; 400; 404 |
| DELETE | `/api/OrdenReparacion/{id}` | 204 o 404 |

Ejemplo de cuerpo para registrar una orden. `fechaIngreso` es opcional y el estado por defecto es Pendiente.

```json
{
  "descripcionProblema": "No enciende",
  "costoEstimado": 120.00,
  "estado": "Pendiente",
  "equipoId": 3,
  "tipoReparacionId": 2
}
```
