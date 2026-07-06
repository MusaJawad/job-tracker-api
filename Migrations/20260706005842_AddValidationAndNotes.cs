using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobTrackerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddValidationAndNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "JobApplications",
                type: "TEXT",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "JobApplications");
        }
    }
}
