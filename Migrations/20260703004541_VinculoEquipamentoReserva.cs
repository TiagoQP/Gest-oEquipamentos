using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gest_oEquipamentos.Migrations
{
    /// <inheritdoc />
    public partial class VinculoEquipamentoReserva : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EquipamentoId",
                table: "Reservas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_EquipamentoId",
                table: "Reservas",
                column: "EquipamentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Equipamentos_EquipamentoId",
                table: "Reservas",
                column: "EquipamentoId",
                principalTable: "Equipamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Equipamentos_EquipamentoId",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_EquipamentoId",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "EquipamentoId",
                table: "Reservas");
        }
    }
}
