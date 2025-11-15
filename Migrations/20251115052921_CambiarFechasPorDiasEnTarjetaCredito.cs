using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlFinanzasProject.Migrations
{
    /// <inheritdoc />
    public partial class CambiarFechasPorDiasEnTarjetaCredito : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaCorte",
                table: "TarjetaCredito");

            migrationBuilder.DropColumn(
                name: "FechaPago",
                table: "TarjetaCredito");

            migrationBuilder.AddColumn<int>(
                name: "DiaCorte",
                table: "TarjetaCredito",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DiaPago",
                table: "TarjetaCredito",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiaCorte",
                table: "TarjetaCredito");

            migrationBuilder.DropColumn(
                name: "DiaPago",
                table: "TarjetaCredito");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCorte",
                table: "TarjetaCredito",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaPago",
                table: "TarjetaCredito",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
