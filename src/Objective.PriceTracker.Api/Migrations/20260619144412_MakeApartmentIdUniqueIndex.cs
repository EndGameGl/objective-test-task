using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Objective.PriceTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class MakeApartmentIdUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_apartment_price_trackers_ApartmentId",
                schema: "price_tracking",
                table: "apartment_price_trackers",
                column: "ApartmentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_apartment_price_trackers_ApartmentId",
                schema: "price_tracking",
                table: "apartment_price_trackers");
        }
    }
}
