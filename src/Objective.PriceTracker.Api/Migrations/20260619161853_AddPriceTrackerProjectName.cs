using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Objective.PriceTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceTrackerProjectName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectName",
                schema: "price_tracking",
                table: "apartment_price_trackers",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectName",
                schema: "price_tracking",
                table: "apartment_price_trackers");
        }
    }
}
