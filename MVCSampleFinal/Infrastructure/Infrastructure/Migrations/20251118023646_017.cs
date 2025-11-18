using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _017 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipo_Sala_SalaId",
                table: "Equipo");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicitud_Equipo_EquipoId",
                table: "Solicitud");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicitud_Sala_SalaId",
                table: "Solicitud");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicitud_Usuarios_UsuarioId",
                table: "Solicitud");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Solicitud",
                table: "Solicitud");

            migrationBuilder.DropIndex(
                name: "IX_Solicitud_EquipoId",
                table: "Solicitud");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sala",
                table: "Sala");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Equipo",
                table: "Equipo");

            migrationBuilder.DropColumn(
                name: "EquipoId",
                table: "Solicitud");

            migrationBuilder.RenameTable(
                name: "Solicitud",
                newName: "Solicitudes");

            migrationBuilder.RenameTable(
                name: "Sala",
                newName: "Salas");

            migrationBuilder.RenameTable(
                name: "Equipo",
                newName: "Equipos");

            migrationBuilder.RenameIndex(
                name: "IX_Solicitud_UsuarioId",
                table: "Solicitudes",
                newName: "IX_Solicitudes_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Solicitud_SalaId",
                table: "Solicitudes",
                newName: "IX_Solicitudes_SalaId");

            migrationBuilder.RenameIndex(
                name: "IX_Equipo_SalaId",
                table: "Equipos",
                newName: "IX_Equipos_SalaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Solicitudes",
                table: "Solicitudes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Salas",
                table: "Salas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Equipos",
                table: "Equipos",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SolicitudesEquipo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Solicitante = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudesEquipo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolicitudesEquipo_Equipos_EquipoId",
                        column: x => x.EquipoId,
                        principalTable: "Equipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitudesEquipo_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesEquipo_EquipoId",
                table: "SolicitudesEquipo",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesEquipo_UsuarioId",
                table: "SolicitudesEquipo",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipos_Salas_SalaId",
                table: "Equipos",
                column: "SalaId",
                principalTable: "Salas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitudes_Salas_SalaId",
                table: "Solicitudes",
                column: "SalaId",
                principalTable: "Salas",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitudes_Usuarios_UsuarioId",
                table: "Solicitudes",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipos_Salas_SalaId",
                table: "Equipos");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicitudes_Salas_SalaId",
                table: "Solicitudes");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicitudes_Usuarios_UsuarioId",
                table: "Solicitudes");

            migrationBuilder.DropTable(
                name: "SolicitudesEquipo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Solicitudes",
                table: "Solicitudes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Salas",
                table: "Salas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Equipos",
                table: "Equipos");

            migrationBuilder.RenameTable(
                name: "Solicitudes",
                newName: "Solicitud");

            migrationBuilder.RenameTable(
                name: "Salas",
                newName: "Sala");

            migrationBuilder.RenameTable(
                name: "Equipos",
                newName: "Equipo");

            migrationBuilder.RenameIndex(
                name: "IX_Solicitudes_UsuarioId",
                table: "Solicitud",
                newName: "IX_Solicitud_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Solicitudes_SalaId",
                table: "Solicitud",
                newName: "IX_Solicitud_SalaId");

            migrationBuilder.RenameIndex(
                name: "IX_Equipos_SalaId",
                table: "Equipo",
                newName: "IX_Equipo_SalaId");

            migrationBuilder.AddColumn<Guid>(
                name: "EquipoId",
                table: "Solicitud",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Solicitud",
                table: "Solicitud",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sala",
                table: "Sala",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Equipo",
                table: "Equipo",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_EquipoId",
                table: "Solicitud",
                column: "EquipoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipo_Sala_SalaId",
                table: "Equipo",
                column: "SalaId",
                principalTable: "Sala",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitud_Equipo_EquipoId",
                table: "Solicitud",
                column: "EquipoId",
                principalTable: "Equipo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitud_Sala_SalaId",
                table: "Solicitud",
                column: "SalaId",
                principalTable: "Sala",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitud_Usuarios_UsuarioId",
                table: "Solicitud",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
