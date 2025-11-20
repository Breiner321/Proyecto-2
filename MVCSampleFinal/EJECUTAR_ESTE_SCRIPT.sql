-- =============================================
-- IMPORTANTE: EJECUTA ESTE SCRIPT EN TU BASE DE DATOS
-- =============================================
-- Este script agrega las columnas necesarias para que funcionen las reservas de salas y equipos
-- 
-- INSTRUCCIONES:
-- 1. Abre SQL Server Management Studio (SSMS) o tu herramienta de base de datos
-- 2. Conéctate a tu base de datos
-- 3. Copia y pega TODO este script
-- 4. Ejecuta el script (F5 o botón Execute)
-- 5. Deberías ver el mensaje "Columnas agregadas correctamente"
-- =============================================

-- Agregar columnas a la tabla Solicitudes
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Solicitudes]') AND name = 'FechaHoraInicio')
BEGIN
    ALTER TABLE [dbo].[Solicitudes]
    ADD [FechaHoraInicio] datetime2 NULL;
    PRINT 'Columna FechaHoraInicio agregada a Solicitudes';
END
ELSE
BEGIN
    PRINT 'La columna FechaHoraInicio ya existe en Solicitudes';
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Solicitudes]') AND name = 'FechaHoraFin')
BEGIN
    ALTER TABLE [dbo].[Solicitudes]
    ADD [FechaHoraFin] datetime2 NULL;
    PRINT 'Columna FechaHoraFin agregada a Solicitudes';
END
ELSE
BEGIN
    PRINT 'La columna FechaHoraFin ya existe en Solicitudes';
END
GO

-- Agregar columnas a la tabla SolicitudesEquipo
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SolicitudesEquipo]') AND name = 'FechaHoraInicio')
BEGIN
    ALTER TABLE [dbo].[SolicitudesEquipo]
    ADD [FechaHoraInicio] datetime2 NULL;
    PRINT 'Columna FechaHoraInicio agregada a SolicitudesEquipo';
END
ELSE
BEGIN
    PRINT 'La columna FechaHoraInicio ya existe en SolicitudesEquipo';
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SolicitudesEquipo]') AND name = 'FechaHoraFin')
BEGIN
    ALTER TABLE [dbo].[SolicitudesEquipo]
    ADD [FechaHoraFin] datetime2 NULL;
    PRINT 'Columna FechaHoraFin agregada a SolicitudesEquipo';
END
ELSE
BEGIN
    PRINT 'La columna FechaHoraFin ya existe en SolicitudesEquipo';
END
GO

PRINT '=============================================';
PRINT 'Script completado. Verifica los mensajes arriba.';
PRINT 'Si todas las columnas ya existían, está todo bien.';
PRINT 'Si se agregaron columnas nuevas, reinicia la aplicación.';
PRINT '=============================================';
GO

-- =============================================
-- CREAR TABLA ReportesDano (si no existe)
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportesDano]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ReportesDano] (
        [Id] uniqueidentifier NOT NULL,
        [Tipo] nvarchar(max) NOT NULL,
        [EquipoId] uniqueidentifier NULL,
        [SalaId] uniqueidentifier NULL,
        [UsuarioId] uniqueidentifier NOT NULL,
        [Descripcion] nvarchar(max) NOT NULL,
        [Fecha] datetime2 NOT NULL,
        [Estado] nvarchar(max) NOT NULL,
        [Observaciones] nvarchar(max) NULL,
        CONSTRAINT [PK_ReportesDano] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ReportesDano_Equipos_EquipoId] FOREIGN KEY ([EquipoId]) 
            REFERENCES [dbo].[Equipos] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_ReportesDano_Salas_SalaId] FOREIGN KEY ([SalaId]) 
            REFERENCES [dbo].[Salas] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_ReportesDano_Usuarios_UsuarioId] FOREIGN KEY ([UsuarioId]) 
            REFERENCES [dbo].[Usuarios] ([Id]) ON DELETE NO ACTION
    );

    CREATE INDEX [IX_ReportesDano_EquipoId] ON [dbo].[ReportesDano] ([EquipoId]);
    CREATE INDEX [IX_ReportesDano_SalaId] ON [dbo].[ReportesDano] ([SalaId]);
    CREATE INDEX [IX_ReportesDano_UsuarioId] ON [dbo].[ReportesDano] ([UsuarioId]);
    
    PRINT 'Tabla ReportesDano creada exitosamente';
END
ELSE
BEGIN
    PRINT 'La tabla ReportesDano ya existe';
END
GO

-- =============================================
-- CREAR TABLA SolicitudesAsesoria (si no existe)
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SolicitudesAsesoria]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[SolicitudesAsesoria] (
        [Id] uniqueidentifier NOT NULL,
        [UsuarioId] uniqueidentifier NOT NULL,
        [Descripcion] nvarchar(max) NOT NULL,
        [TipoAsesoria] nvarchar(max) NOT NULL,
        [Fecha] datetime2 NOT NULL,
        [FechaHoraSolicitada] datetime2 NULL,
        [Estado] nvarchar(max) NOT NULL,
        [Observaciones] nvarchar(max) NULL,
        [Solicitante] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_SolicitudesAsesoria] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SolicitudesAsesoria_Usuarios_UsuarioId] FOREIGN KEY ([UsuarioId]) 
            REFERENCES [dbo].[Usuarios] ([Id]) ON DELETE NO ACTION
    );

    CREATE INDEX [IX_SolicitudesAsesoria_UsuarioId] ON [dbo].[SolicitudesAsesoria] ([UsuarioId]);
    
    PRINT 'Tabla SolicitudesAsesoria creada exitosamente';
END
ELSE
BEGIN
    PRINT 'La tabla SolicitudesAsesoria ya existe';
END
GO

PRINT '=============================================';
PRINT 'TODOS LOS SCRIPTS COMPLETADOS.';
PRINT 'Reinicia la aplicación para que los cambios surtan efecto.';
PRINT '=============================================';

