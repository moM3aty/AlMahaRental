using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlMahaRental.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBranchLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MapUrl",
                table: "BranchLocations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "BranchLocations",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WorkingHours",
                table: "BranchLocations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MapUrl",
                table: "BranchLocations");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "BranchLocations");

            migrationBuilder.DropColumn(
                name: "WorkingHours",
                table: "BranchLocations");
        }
    }
}
