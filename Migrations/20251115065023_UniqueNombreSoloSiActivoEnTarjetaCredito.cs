using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlFinanzasProject.Migrations
{
    /// <inheritdoc />
    public partial class UniqueNombreSoloSiActivoEnTarjetaCredito : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TarjetaCredito_Nombre_EsActivo",
                table: "TarjetaCredito");

            migrationBuilder.CreateIndex(
                name: "IX_TarjetaCredito_Nombre_EsActivo",
                table: "TarjetaCredito",
                columns: new[] { "Nombre", "EsActivo" },
                unique: true,
                filter: "[EsActivo] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TarjetaCredito_Nombre_EsActivo",
                table: "TarjetaCredito");

            migrationBuilder.CreateIndex(
                name: "IX_TarjetaCredito_Nombre_EsActivo",
                table: "TarjetaCredito",
                columns: new[] { "Nombre", "EsActivo" },
                unique: true);
        }
    }
}
