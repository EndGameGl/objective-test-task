using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Objective.PriceTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "price_tracking");

            migrationBuilder.CreateTable(
                name: "apartment_price_trackers",
                schema: "price_tracking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApartmentId = table.Column<int>(type: "integer", nullable: false),
                    LastPrice = table.Column<double>(type: "double precision", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_apartment_price_trackers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "subscribers",
                schema: "price_tracking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubscriberMail = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subscribers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "subscriptions",
                schema: "price_tracking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubscriberId = table.Column<int>(type: "integer", nullable: false),
                    ApartmentTrackerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_subscriptions_apartment_price_trackers_ApartmentTrackerId",
                        column: x => x.ApartmentTrackerId,
                        principalSchema: "price_tracking",
                        principalTable: "apartment_price_trackers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_subscriptions_subscribers_SubscriberId",
                        column: x => x.SubscriberId,
                        principalSchema: "price_tracking",
                        principalTable: "subscribers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_subscribers_SubscriberMail",
                schema: "price_tracking",
                table: "subscribers",
                column: "SubscriberMail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_subscriptions_ApartmentTrackerId",
                schema: "price_tracking",
                table: "subscriptions",
                column: "ApartmentTrackerId");

            migrationBuilder.CreateIndex(
                name: "IX_subscriptions_SubscriberId_ApartmentTrackerId",
                schema: "price_tracking",
                table: "subscriptions",
                columns: new[] { "SubscriberId", "ApartmentTrackerId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "subscriptions",
                schema: "price_tracking");

            migrationBuilder.DropTable(
                name: "apartment_price_trackers",
                schema: "price_tracking");

            migrationBuilder.DropTable(
                name: "subscribers",
                schema: "price_tracking");
        }
    }
}
