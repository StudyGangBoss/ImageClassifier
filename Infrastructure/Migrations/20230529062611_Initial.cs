using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "imageclassifier");

            migrationBuilder.CreateTable(
                name: "classification_types",
                schema: "imageclassifier",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Class = table.Column<string>(type: "text", nullable: false),
                    Question = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classification_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "images",
                schema: "imageclassifier",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ImageData = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "imageclassifier",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatId = table.Column<long>(type: "bigint", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "image_classifications",
                schema: "imageclassifier",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassificationTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Mark = table.Column<int>(type: "integer", nullable: false),
                    ImageId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_image_classifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_image_classifications_classification_types_ClassificationTy~",
                        column: x => x.ClassificationTypeId,
                        principalSchema: "imageclassifier",
                        principalTable: "classification_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_image_classifications_images_ImageId",
                        column: x => x.ImageId,
                        principalSchema: "imageclassifier",
                        principalTable: "images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_image_classifications_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "imageclassifier",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_image_classifications_ClassificationTypeId",
                schema: "imageclassifier",
                table: "image_classifications",
                column: "ClassificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_image_classifications_ImageId",
                schema: "imageclassifier",
                table: "image_classifications",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_image_classifications_UserId",
                schema: "imageclassifier",
                table: "image_classifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_users_ChatId",
                schema: "imageclassifier",
                table: "users",
                column: "ChatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "image_classifications",
                schema: "imageclassifier");

            migrationBuilder.DropTable(
                name: "classification_types",
                schema: "imageclassifier");

            migrationBuilder.DropTable(
                name: "images",
                schema: "imageclassifier");

            migrationBuilder.DropTable(
                name: "users",
                schema: "imageclassifier");
        }
    }
}
