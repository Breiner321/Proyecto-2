-- Script para crear la tabla ReportesDano manualmente
-- Ejecuta este script en tu base de datos SQL Server

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

