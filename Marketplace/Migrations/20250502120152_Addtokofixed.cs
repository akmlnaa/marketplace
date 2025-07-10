using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketplace.Migrations
{
    /// <inheritdoc />
    public partial class Addtokofixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<int>(
                name: "TokoId",
                table: "Ikans",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tokos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NamaToko = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Alamat = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PenjualId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tokos_Users_PenjualId",
                        column: x => x.PenjualId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaksis_IkanId",
                table: "Transaksis",
                column: "IkanId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaksis_PembeliId",
                table: "Transaksis",
                column: "PembeliId");

            migrationBuilder.CreateIndex(
                name: "IX_Ikans_PenjualId",
                table: "Ikans",
                column: "PenjualId");

            migrationBuilder.CreateIndex(
                name: "IX_Ikans_TokoId",
                table: "Ikans",
                column: "TokoId");

            migrationBuilder.CreateIndex(
                name: "IX_Tokos_PenjualId",
                table: "Tokos",
                column: "PenjualId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ikans_Tokos_TokoId",
                table: "Ikans",
                column: "TokoId",
                principalTable: "Tokos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Ikans_Users_PenjualId",
                table: "Ikans",
                column: "PenjualId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaksis_Ikans_IkanId",
                table: "Transaksis",
                column: "IkanId",
                principalTable: "Ikans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaksis_Users_PembeliId",
                table: "Transaksis",
                column: "PembeliId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ikans_Tokos_TokoId",
                table: "Ikans");

            migrationBuilder.DropForeignKey(
                name: "FK_Ikans_Users_PenjualId",
                table: "Ikans");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaksis_Ikans_IkanId",
                table: "Transaksis");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaksis_Users_PembeliId",
                table: "Transaksis");

            migrationBuilder.DropTable(
                name: "Tokos");

            migrationBuilder.DropIndex(
                name: "IX_Transaksis_IkanId",
                table: "Transaksis");

            migrationBuilder.DropIndex(
                name: "IX_Transaksis_PembeliId",
                table: "Transaksis");

            migrationBuilder.DropIndex(
                name: "IX_Ikans_PenjualId",
                table: "Ikans");

            migrationBuilder.DropIndex(
                name: "IX_Ikans_TokoId",
                table: "Ikans");

            
            migrationBuilder.DropColumn(
                name: "TokoId",
                table: "Ikans");
        }
    }
}
