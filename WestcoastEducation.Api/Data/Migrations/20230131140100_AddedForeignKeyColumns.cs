using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WestcoastEducationRESTDel2.api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedForeignKeyColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "Courses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Courses");
        }
    }
}
