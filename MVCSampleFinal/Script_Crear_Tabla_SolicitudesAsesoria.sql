-- Script para crear la tabla SolicitudesAsesoria manualmente
-- Ejecuta este script en tu base de datos SQL Server
-- Base de datos: db_ac0136_universidad

USE [db_ac0136_universidad];
GO

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
            REFERENCES [dbo].[Usuarios] ([Id]) ON DELETE CASCADE
    );

    CREATE INDEX [IX_SolicitudesAsesoria_UsuarioId] ON [dbo].[SolicitudesAsesoria] ([UsuarioId]);
    
    PRINT 'Tabla SolicitudesAsesoria creada exitosamente';
END
ELSE
BEGIN
    PRINT 'La tabla SolicitudesAsesoria ya existe';
END
GO

