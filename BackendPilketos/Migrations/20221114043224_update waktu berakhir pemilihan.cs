using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendPilketos.Migrations
{
    public partial class updatewaktuberakhirpemilihan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WaktuBerakhir",
                table: "Periodes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WaktuBerakhir",
                table: "Periodes");
        }
    }
}
