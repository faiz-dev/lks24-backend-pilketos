using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendPilketos.Migrations
{
    public partial class tambahPeriodedicalon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserGroups_GroupId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PeriodeId",
                table: "Calons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Calons_PeriodeId",
                table: "Calons",
                column: "PeriodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Calons_Periodes_PeriodeId",
                table: "Calons",
                column: "PeriodeId",
                principalTable: "Periodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserGroups_GroupId",
                table: "Users",
                column: "GroupId",
                principalTable: "UserGroups",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calons_Periodes_PeriodeId",
                table: "Calons");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserGroups_GroupId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Calons_PeriodeId",
                table: "Calons");

            migrationBuilder.DropColumn(
                name: "PeriodeId",
                table: "Calons");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserGroups_GroupId",
                table: "Users",
                column: "GroupId",
                principalTable: "UserGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
