using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.Migrations
{
    /// <inheritdoc />
    public partial class somechange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModuleName",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Value",
                table: "ModuleDatas",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModuleName",
                table: "Modules");

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "ModuleDatas",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
