using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFechaHoraColumnsToSolicitudes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Agregar columnas a la tabla Solicitudes
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaHoraInicio",
                table: "Solicitudes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaHoraFin",
                table: "Solicitudes",
                type: "datetime2",
                nullable: true);

            // Agregar columnas a la tabla SolicitudesEquipo
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaHoraInicio",
                table: "SolicitudesEquipo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaHoraFin",
                table: "SolicitudesEquipo",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eliminar columnas de la tabla SolicitudesEquipo
            migrationBuilder.DropColumn(
                name: "FechaHoraFin",
                table: "SolicitudesEquipo");

            migrationBuilder.DropColumn(
                name: "FechaHoraInicio",
                table: "SolicitudesEquipo");

            // Eliminar columnas de la tabla Solicitudes
            migrationBuilder.DropColumn(
                name: "FechaHoraFin",
                table: "Solicitudes");

            migrationBuilder.DropColumn(
                name: "FechaHoraInicio",
                table: "Solicitudes");
        }
    }
}

