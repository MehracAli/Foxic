using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOXIC.Migrations
{
    public partial class ClothiingIdInBasketItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClothingId",
                table: "BasketItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClothingId",
                table: "BasketItems");
        }
    }
}
