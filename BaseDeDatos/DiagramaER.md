# Diagrama entidad-relación

Modelo de la base de datos `ServicioTecnicoDB`, definido en [ServicioTecnicoDB.sql](ServicioTecnicoDB.sql). GitHub
muestra el diagrama al abrir este archivo. Para verlo en https://mermaid.live, pegue solo el código del diagrama,
desde la línea erDiagram hasta la última llave, sin las líneas de apertura y cierre del bloque.

```mermaid
erDiagram
    Cliente ||--o{ Equipo : "posee"
    Equipo ||--o{ OrdenReparacion : "tiene"
    TipoReparacion ||--o{ OrdenReparacion : "clasifica"

    Cliente {
        int Id PK "IDENTITY(1,1)"
        nvarchar(50) Paterno
        nvarchar(50) Materno "NULL"
        nvarchar(100) Nombres
        nvarchar(150) Correo "NULL"
        nvarchar(20) Telefono "NULL"
    }

    Equipo {
        int Id PK "IDENTITY(1,1)"
        nvarchar(50) NumeroSerie UK
        nvarchar(50) Marca
        nvarchar(50) Modelo
        int Anio "NULL, entre 1990 y 2100"
        int ClienteId FK
    }

    TipoReparacion {
        int Id PK "IDENTITY(1,1)"
        nvarchar(100) Nombre
        decimal PrecioBase "DECIMAL(10,2), no negativo"
    }

    OrdenReparacion {
        int Id PK "IDENTITY(1,1)"
        datetime2 FechaIngreso "DATETIME2(0), por defecto SYSDATETIME()"
        nvarchar(500) DescripcionProblema
        decimal CostoEstimado "DECIMAL(10,2), no negativo"
        nvarchar(20) Estado "Pendiente, En proceso, Reparado, Entregado o Cancelado"
        int EquipoId FK
        int TipoReparacionId FK
    }
```

| Relación | Cardinalidad | Clave foránea |
|---|---|---|
| Cliente y Equipo | Un cliente tiene cero o más equipos; cada equipo pertenece a un cliente. | `FK_Equipo_Cliente` |
| Equipo y OrdenReparacion | Un equipo tiene cero o más órdenes; cada orden corresponde a un equipo. | `FK_OrdenReparacion_Equipo` |
| TipoReparacion y OrdenReparacion | Un tipo de reparación clasifica cero o más órdenes; cada orden tiene un tipo. | `FK_OrdenReparacion_TipoReparacion` |
