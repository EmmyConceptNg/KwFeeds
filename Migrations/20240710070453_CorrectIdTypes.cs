using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DancingGoat.Migrations
{
    public partial class CorrectIdTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abouts",
                columns: table => new
                {
                    tag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    button_text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "AssetDimensions",
                columns: table => new
                {
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "AssetRelatedItem",
                columns: table => new
                {
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ContentItemFields",
                columns: table => new
                {
                    ContentItemID = table.Column<int>(type: "int", nullable: false),
                    ContentItemGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContentItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentItemIsSecured = table.Column<bool>(type: "bit", nullable: false),
                    ContentItemContentTypeID = table.Column<int>(type: "int", nullable: false),
                    ContentItemCommonDataContentLanguageID = table.Column<int>(type: "int", nullable: false),
                    ContentItemCommonDataVersionStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "HomePageBanners",
                columns: table => new
                {
                    tag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    button_text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "SingleProducts",
                columns: table => new
                {
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nutritional_information = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ingredients = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    feeding_guidelines = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    usage_tips = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    storage_instructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    health_benefits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsProductOfTheMonth = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "SiteSettings",
                columns: table => new
                {
                    HomePageTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abouts");

            migrationBuilder.DropTable(
                name: "AssetDimensions");

            migrationBuilder.DropTable(
                name: "AssetRelatedItem");

            migrationBuilder.DropTable(
                name: "ContentItemFields");

            migrationBuilder.DropTable(
                name: "HomePageBanners");

            migrationBuilder.DropTable(
                name: "SingleProducts");

            migrationBuilder.DropTable(
                name: "SiteSettings");
        }
    }
}
