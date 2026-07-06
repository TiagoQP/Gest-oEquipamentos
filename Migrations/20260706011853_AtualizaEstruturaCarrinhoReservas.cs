using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gest_oEquipamentos.Migrations
{
    /// <inheritdoc />
    public partial class AtualizaEstruturaCarrinhoReservas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Reservas",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateTable(
                name: "ItensReserva",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReservaId = table.Column<int>(type: "INTEGER", nullable: false),
                    EquipamentoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantidade = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensReserva", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensReserva_Equipamentos_EquipamentoId",
                        column: x => x.EquipamentoId,
                        principalTable: "Equipamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItensReserva_Reservas_ReservaId",
                        column: x => x.ReservaId,
                        principalTable: "Reservas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItensReserva_EquipamentoId",
                table: "ItensReserva",
                column: "EquipamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensReserva_ReservaId",
                table: "ItensReserva",
                column: "ReservaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItensReserva");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Reservas",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

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
    }
}
