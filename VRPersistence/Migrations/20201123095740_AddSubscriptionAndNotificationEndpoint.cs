using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace VRPersistence.Migrations
{
    public partial class AddSubscriptionAndNotificationEndpoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationEndpoints",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Identifier = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationEndpoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MediaId = table.Column<long>(nullable: false),
                    NotificationEndpointId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscriptions_NotificationEndpoints_NotificationEndpointId",
                        column: x => x.NotificationEndpointId,
                        principalTable: "NotificationEndpoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Media_MediaName",
                table: "Media",
                column: "MediaName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationEndpoints_Identifier",
                table: "NotificationEndpoints",
                column: "Identifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_NotificationEndpointId",
                table: "Subscriptions",
                column: "NotificationEndpointId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_MediaId_NotificationEndpointId",
                table: "Subscriptions",
                columns: new[] { "MediaId", "NotificationEndpointId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "NotificationEndpoints");

            migrationBuilder.DropIndex(
                name: "IX_Media_MediaName",
                table: "Media");
        }
    }
}
