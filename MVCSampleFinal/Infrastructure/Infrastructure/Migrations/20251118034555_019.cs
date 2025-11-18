using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _019 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitudes_Salas_SalaId",
                table: "Solicitudes");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitudes_Salas_SalaId",
                table: "Solicitudes",
                column: "SalaId",
                principalTable: "Salas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitudes_Salas_SalaId",
                table: "Solicitudes");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitudes_Salas_SalaId",
                table: "Solicitudes",
                column: "SalaId",
                principalTable: "Salas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
