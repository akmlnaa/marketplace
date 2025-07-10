using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketplace.Migrations
{
    /// <inheritdoc />
    public partial class AddTokoIdToIkan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ikans_Tokos_TokoId",
                table: "Ikans");

            migrationBuilder.DropForeignKey(
                name: "FK_Tokos_Users_PenjualId",
                table: "Tokos");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaksis_Ikans_IkanId",
                table: "Transaksis");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaksis_Users_PembeliId",
                table: "Transaksis");

            migrationBuilder.DropIndex(
                name: "IX_Transaksis_IkanId",
                table: "Transaksis");

            migrationBuilder.DropIndex(
                name: "IX_Transaksis_PembeliId",
                table: "Transaksis");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tokos",
                table: "Tokos");

            migrationBuilder.AlterColumn<string>(
                name: "NamaToko",
                table: "Tokos",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Alamat",
                table: "Tokos",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "Deskripsi",
                table: "Tokos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Deskripsi",
                table: "Ikans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gambar",
                table: "Ikans",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TanggalPublish",
                table: "Ikans",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__Tokos__3214EC07AF922A9D",
                table: "Tokos",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Pembayarans",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransaksiID = table.Column<int>(type: "int", nullable: false),
                    TglPembayaran = table.Column<DateTime>(type: "datetime", nullable: false),
                    MetodePembayaran = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JumlahBayar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BuktiBayar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Pembayar__3214EC27F7C30285", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Pembayaran_Transaksi",
                        column: x => x.TransaksiID,
                        principalTable: "Transaksis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pembayarans_TransaksiID",
                table: "Pembayarans",
                column: "TransaksiID");

            migrationBuilder.AddForeignKey(
                name: "FK_Ikans_Tokos_TokoId",
                table: "Ikans",
                column: "TokoId",
                principalTable: "Tokos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Toko_User",
                table: "Tokos",
                column: "PenjualId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ikans_Tokos_TokoId",
                table: "Ikans");

            migrationBuilder.DropForeignKey(
                name: "FK_Toko_User",
                table: "Tokos");

            migrationBuilder.DropTable(
                name: "Pembayarans");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Tokos__3214EC07AF922A9D",
                table: "Tokos");

            migrationBuilder.DropColumn(
                name: "Deskripsi",
                table: "Tokos");

            migrationBuilder.DropColumn(
                name: "Deskripsi",
                table: "Ikans");

            migrationBuilder.DropColumn(
                name: "Gambar",
                table: "Ikans");

            migrationBuilder.DropColumn(
                name: "TanggalPublish",
                table: "Ikans");

            migrationBuilder.AlterColumn<string>(
                name: "NamaToko",
                table: "Tokos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Alamat",
                table: "Tokos",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldUnicode: false,
                oldMaxLength: 200);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tokos",
                table: "Tokos",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Transaksis_IkanId",
                table: "Transaksis",
                column: "IkanId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaksis_PembeliId",
                table: "Transaksis",
                column: "PembeliId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ikans_Tokos_TokoId",
                table: "Ikans",
                column: "TokoId",
                principalTable: "Tokos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Tokos_Users_PenjualId",
                table: "Tokos",
                column: "PenjualId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
    }
}
