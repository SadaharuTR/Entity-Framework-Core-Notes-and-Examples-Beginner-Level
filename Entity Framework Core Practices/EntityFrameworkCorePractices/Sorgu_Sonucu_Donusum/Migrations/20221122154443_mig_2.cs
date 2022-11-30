using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SorguSonucuDonusum.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UrunDetaylari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fiyat = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunDetaylari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Urunler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrunAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fiyat = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Urunler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parcalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParcaAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrunId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parcalar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parcalar_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UrunParcalari",
                columns: table => new
                {
                    UrunId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParcaId = table.Column<float>(type: "real", nullable: false),
                    UrunId1 = table.Column<int>(type: "int", nullable: false),
                    ParcaId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunParcalari", x => x.UrunId);
                    table.ForeignKey(
                        name: "FK_UrunParcalari_Parcalar_ParcaId1",
                        column: x => x.ParcaId1,
                        principalTable: "Parcalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UrunParcalari_Urunler_UrunId1",
                        column: x => x.UrunId1,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parcalar_UrunId",
                table: "Parcalar",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunParcalari_ParcaId1",
                table: "UrunParcalari",
                column: "ParcaId1");

            migrationBuilder.CreateIndex(
                name: "IX_UrunParcalari_UrunId1",
                table: "UrunParcalari",
                column: "UrunId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrunDetaylari");

            migrationBuilder.DropTable(
                name: "UrunParcalari");

            migrationBuilder.DropTable(
                name: "Parcalar");

            migrationBuilder.DropTable(
                name: "Urunler");
        }
    }
}
