/*
  ServicioTecnicoDB: base de datos para la gestión de un servicio técnico de equipos electrónicos.

  Relaciones:
    Cliente 1 --- * Equipo 1 --- * OrdenReparacion * --- 1 TipoReparacion

  - Claves primarias IDENTITY(1,1), generadas por SQL Server.
  - Claves foráneas con nombre y con un índice cada una.
  - Restricciones CHECK para precios y costos no negativos, años entre 1990 y 2100 y estados de orden válidos.
  - Script idempotente: crea la base, las tablas y los datos iniciales solo si no existen.
*/
IF DB_ID(N'ServicioTecnicoDB') IS NULL
    CREATE DATABASE ServicioTecnicoDB;
GO

USE ServicioTecnicoDB;
GO

IF OBJECT_ID(N'dbo.TipoReparacion', N'U') IS NULL
CREATE TABLE dbo.TipoReparacion
(
    Id          INT            IDENTITY(1, 1) NOT NULL CONSTRAINT PK_TipoReparacion PRIMARY KEY,
    Nombre      NVARCHAR(100)  NOT NULL,
    PrecioBase  DECIMAL(10, 2) NOT NULL CONSTRAINT CK_TipoReparacion_PrecioBase CHECK (PrecioBase >= 0)
);
GO

IF OBJECT_ID(N'dbo.Cliente', N'U') IS NULL
CREATE TABLE dbo.Cliente
(
    Id        INT           IDENTITY(1, 1) NOT NULL CONSTRAINT PK_Cliente PRIMARY KEY,
    Paterno   NVARCHAR(50)  NOT NULL,
    Materno   NVARCHAR(50)  NULL,
    Nombres   NVARCHAR(100) NOT NULL,
    Correo    NVARCHAR(150) NULL,
    Telefono  NVARCHAR(20)  NULL
);
GO

IF OBJECT_ID(N'dbo.Equipo', N'U') IS NULL
CREATE TABLE dbo.Equipo
(
    Id           INT          IDENTITY(1, 1) NOT NULL CONSTRAINT PK_Equipo PRIMARY KEY,
    NumeroSerie  NVARCHAR(50) NOT NULL CONSTRAINT UQ_Equipo_NumeroSerie UNIQUE,
    Marca        NVARCHAR(50) NOT NULL,
    Modelo       NVARCHAR(50) NOT NULL,
    Anio         INT          NULL CONSTRAINT CK_Equipo_Anio CHECK (Anio BETWEEN 1990 AND 2100),
    ClienteId    INT          NOT NULL CONSTRAINT FK_Equipo_Cliente REFERENCES dbo.Cliente (Id)
);
GO

IF OBJECT_ID(N'dbo.OrdenReparacion', N'U') IS NULL
CREATE TABLE dbo.OrdenReparacion
(
    Id                   INT            IDENTITY(1, 1) NOT NULL CONSTRAINT PK_OrdenReparacion PRIMARY KEY,
    FechaIngreso         DATETIME2(0)   NOT NULL CONSTRAINT DF_OrdenReparacion_FechaIngreso DEFAULT (SYSDATETIME()),
    DescripcionProblema  NVARCHAR(500)  NOT NULL,
    CostoEstimado        DECIMAL(10, 2) NOT NULL CONSTRAINT CK_OrdenReparacion_CostoEstimado CHECK (CostoEstimado >= 0),
    Estado               NVARCHAR(20)   NOT NULL CONSTRAINT DF_OrdenReparacion_Estado DEFAULT (N'Pendiente')
                                        CONSTRAINT CK_OrdenReparacion_Estado
                                        CHECK (Estado IN (N'Pendiente', N'En proceso', N'Reparado', N'Entregado', N'Cancelado')),
    EquipoId             INT            NOT NULL CONSTRAINT FK_OrdenReparacion_Equipo REFERENCES dbo.Equipo (Id),
    TipoReparacionId     INT            NOT NULL CONSTRAINT FK_OrdenReparacion_TipoReparacion REFERENCES dbo.TipoReparacion (Id)
);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Equipo_ClienteId')
    CREATE INDEX IX_Equipo_ClienteId ON dbo.Equipo (ClienteId);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_OrdenReparacion_EquipoId')
    CREATE INDEX IX_OrdenReparacion_EquipoId ON dbo.OrdenReparacion (EquipoId);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_OrdenReparacion_TipoReparacionId')
    CREATE INDEX IX_OrdenReparacion_TipoReparacionId ON dbo.OrdenReparacion (TipoReparacionId);
GO

-- Datos iniciales de prueba (solo si las tablas están vacías).
IF NOT EXISTS (SELECT 1 FROM dbo.TipoReparacion)
    INSERT INTO dbo.TipoReparacion (Nombre, PrecioBase)
    VALUES (N'Cambio de pantalla', 180.00),
           (N'Cambio de batería', 90.00),
           (N'Mantenimiento general', 60.00);

IF NOT EXISTS (SELECT 1 FROM dbo.Cliente)
    INSERT INTO dbo.Cliente (Paterno, Materno, Nombres, Correo, Telefono)
    VALUES (N'Quispe', N'Mamani', N'Rosa', N'rosa.quispe@correo.pe', N'987654321'),
           (N'Torres', N'Salas', N'Luis', N'luis.torres@correo.pe', N'912345678');

IF NOT EXISTS (SELECT 1 FROM dbo.Equipo)
    INSERT INTO dbo.Equipo (NumeroSerie, Marca, Modelo, Anio, ClienteId)
    VALUES (N'SN-A1001', N'Samsung', N'Galaxy A54', 2023, 1),
           (N'SN-B2002', N'Lenovo', N'IdeaPad 3', 2022, 2),
           (N'SN-C3003', N'Apple', N'iPhone 13', 2021, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.OrdenReparacion)
    INSERT INTO dbo.OrdenReparacion (FechaIngreso, DescripcionProblema, CostoEstimado, Estado, EquipoId, TipoReparacionId)
    VALUES ('2026-10-01T09:30:00', N'La pantalla está rota y no responde al tacto', 180.00, N'Pendiente', 1, 1),
           ('2026-10-03T15:10:00', N'Se calienta y se apaga solo', 75.50, N'En proceso', 2, 3);
GO
