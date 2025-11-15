using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlFinanzasProject.Migrations
{
    /// <inheritdoc />
    public partial class UniqueNombreEsActivoEnTarjetaCredito : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TarjetaCredito_Nombre",
                table: "TarjetaCredito");

            migrationBuilder.CreateIndex(
                name: "IX_TarjetaCredito_Nombre_EsActivo",
                table: "TarjetaCredito",
                columns: new[] { "Nombre", "EsActivo" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TarjetaCredito_Nombre_EsActivo",
                table: "TarjetaCredito");

            migrationBuilder.CreateIndex(
                name: "IX_TarjetaCredito_Nombre",
                table: "TarjetaCredito",
                column: "Nombre",
                unique: true);
        }
    }
}
