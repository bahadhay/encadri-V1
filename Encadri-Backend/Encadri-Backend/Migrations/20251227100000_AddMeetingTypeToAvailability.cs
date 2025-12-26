using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Encadri_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddMeetingTypeToAvailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MeetingType",
                table: "SupervisorAvailabilities",
                type: "text",
                nullable: true,
                defaultValue: "both");

            // Also add MeetingType to MeetingRequests if it doesn't exist
            migrationBuilder.AddColumn<string>(
                name: "MeetingType",
                table: "MeetingRequests",
                type: "text",
                nullable: true,
                defaultValue: "virtual");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "MeetingRequests",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeetingType",
                table: "SupervisorAvailabilities");

            migrationBuilder.DropColumn(
                name: "MeetingType",
                table: "MeetingRequests");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "MeetingRequests");
        }
    }
}
