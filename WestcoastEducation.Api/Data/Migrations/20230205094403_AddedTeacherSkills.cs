using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WestcoastEducationRESTDel2.api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedTeacherSkills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Teachers_TeacherModelId",
                table: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skills",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_TeacherModelId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "TeacherModelId",
                table: "Skills");

            migrationBuilder.RenameTable(
                name: "Skills",
                newName: "TeacherSkills");

            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "TeacherSkills",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeacherSkills",
                table: "TeacherSkills",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSkills_TeacherId",
                table: "TeacherSkills",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSkills_Teachers_TeacherId",
                table: "TeacherSkills",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSkills_Teachers_TeacherId",
                table: "TeacherSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeacherSkills",
                table: "TeacherSkills");

            migrationBuilder.DropIndex(
                name: "IX_TeacherSkills_TeacherId",
                table: "TeacherSkills");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "TeacherSkills");

            migrationBuilder.RenameTable(
                name: "TeacherSkills",
                newName: "Skills");

            migrationBuilder.AddColumn<int>(
                name: "TeacherModelId",
                table: "Skills",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skills",
                table: "Skills",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_TeacherModelId",
                table: "Skills",
                column: "TeacherModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Teachers_TeacherModelId",
                table: "Skills",
                column: "TeacherModelId",
                principalTable: "Teachers",
                principalColumn: "Id");
        }
    }
}
