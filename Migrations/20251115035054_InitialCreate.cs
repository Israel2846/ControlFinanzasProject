using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlFinanzasProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TarjetaCredito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LimiteCredito = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DeudaActual = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FechaCorte = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaPago = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EsActivo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TarjetaCredito", x => x.Id);
                    table.CheckConstraint("CK_TarjetaCredito_DeudaActual", "[DeudaActual] >= 0 AND [DeudaActual] <= [LimiteCredito]");
                });

            migrationBuilder.CreateTable(
                name: "TipoMovimiento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EsActivo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoMovimiento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoriaMovimiento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EsActivo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    TipoMovimientoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaMovimiento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoriaMovimiento_TipoMovimiento_TipoMovimientoId",
                        column: x => x.TipoMovimientoId,
                        principalTable: "TipoMovimiento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Movimiento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    SaldoAnterior = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SaldoPosterior = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    EsActivo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    EsEfectivo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CategoriaMovimientoId = table.Column<int>(type: "int", nullable: false),
                    TarjetaCreditoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimiento", x => x.Id);
                    table.CheckConstraint("CK_Movimiento_Monto", "[Monto] > 0");
                    table.ForeignKey(
                        name: "FK_Movimiento_CategoriaMovimiento_CategoriaMovimientoId",
                        column: x => x.CategoriaMovimientoId,
                        principalTable: "CategoriaMovimiento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Movimiento_TarjetaCredito_TarjetaCreditoId",
                        column: x => x.TarjetaCreditoId,
                        principalTable: "TarjetaCredito",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PagoTarjeta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaPago = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MontoPago = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    EsActivo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    TarjetaCreditoId = table.Column<int>(type: "int", nullable: false),
                    MovimientoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagoTarjeta", x => x.Id);
                    table.CheckConstraint("CK_PagoTarjeta_MontoPago", "[MontoPago] > 0");
                    table.ForeignKey(
                        name: "FK_PagoTarjeta_Movimiento_MovimientoId",
                        column: x => x.MovimientoId,
                        principalTable: "Movimiento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PagoTarjeta_TarjetaCredito_TarjetaCreditoId",
                        column: x => x.TarjetaCreditoId,
                        principalTable: "TarjetaCredito",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaMovimiento_EsActivo",
                table: "CategoriaMovimiento",
                column: "EsActivo");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaMovimiento_TipoMovimientoId",
                table: "CategoriaMovimiento",
                column: "TipoMovimientoId");

            migrationBuilder.CreateIndex(
                name: "IX_Movimiento_CategoriaMovimientoId",
                table: "Movimiento",
                column: "CategoriaMovimientoId");

            migrationBuilder.CreateIndex(
                name: "IX_Movimiento_CategoriaMovimientoId_EsActivo",
                table: "Movimiento",
                columns: new[] { "CategoriaMovimientoId", "EsActivo" });

            migrationBuilder.CreateIndex(
                name: "IX_Movimiento_EsActivo",
                table: "Movimiento",
                column: "EsActivo");

            migrationBuilder.CreateIndex(
                name: "IX_Movimiento_EsEfectivo",
                table: "Movimiento",
                column: "EsEfectivo");

            migrationBuilder.CreateIndex(
                name: "IX_Movimiento_Fecha",
                table: "Movimiento",
                column: "Fecha");

            migrationBuilder.CreateIndex(
                name: "IX_Movimiento_Fecha_EsActivo",
                table: "Movimiento",
                columns: new[] { "Fecha", "EsActivo" });

            migrationBuilder.CreateIndex(
                name: "IX_Movimiento_TarjetaCreditoId",
                table: "Movimiento",
                column: "TarjetaCreditoId");

            migrationBuilder.CreateIndex(
                name: "IX_PagoTarjeta_FechaPago",
                table: "PagoTarjeta",
                column: "FechaPago");

            migrationBuilder.CreateIndex(
                name: "IX_PagoTarjeta_MovimientoId",
                table: "PagoTarjeta",
                column: "MovimientoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PagoTarjeta_TarjetaCreditoId",
                table: "PagoTarjeta",
                column: "TarjetaCreditoId");

            migrationBuilder.CreateIndex(
                name: "IX_TarjetaCredito_EsActivo",
                table: "TarjetaCredito",
                column: "EsActivo");

            migrationBuilder.CreateIndex(
                name: "IX_TarjetaCredito_Nombre",
                table: "TarjetaCredito",
                column: "Nombre",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PagoTarjeta");

            migrationBuilder.DropTable(
                name: "Movimiento");

            migrationBuilder.DropTable(
                name: "CategoriaMovimiento");

            migrationBuilder.DropTable(
                name: "TarjetaCredito");

            migrationBuilder.DropTable(
                name: "TipoMovimiento");
        }
    }
}
