-- Script SQL para agregar las columnas FechaHoraInicio y FechaHoraFin
-- Ejecuta este script en tu base de datos SQL Server

-- Agregar columnas a la tabla Solicitudes
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Solicitudes]') AND name = 'FechaHoraInicio')
BEGIN
    ALTER TABLE [dbo].[Solicitudes]
    ADD [FechaHoraInicio] datetime2 NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Solicitudes]') AND name = 'FechaHoraFin')
BEGIN
    ALTER TABLE [dbo].[Solicitudes]
    ADD [FechaHoraFin] datetime2 NULL;
END
GO

-- Agregar columnas a la tabla SolicitudesEquipo
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SolicitudesEquipo]') AND name = 'FechaHoraInicio')
BEGIN
    ALTER TABLE [dbo].[SolicitudesEquipo]
    ADD [FechaHoraInicio] datetime2 NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SolicitudesEquipo]') AND name = 'FechaHoraFin')
BEGIN
    ALTER TABLE [dbo].[SolicitudesEquipo]
    ADD [FechaHoraFin] datetime2 NULL;
END
GO

PRINT 'Columnas agregadas correctamente';

