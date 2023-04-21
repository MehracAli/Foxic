using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FOXIC.Migrations
{
    public partial class nullableCS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItems_ClothingColorSizes_ClothingColorSizeId",
                table: "BasketItems");

            migrationBuilder.AlterColumn<int>(
                name: "ClothingColorSizeId",
                table: "BasketItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_ClothingColorSizes_ClothingColorSizeId",
                table: "BasketItems",
                column: "ClothingColorSizeId",
                principalTable: "ClothingColorSizes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItems_ClothingColorSizes_ClothingColorSizeId",
                table: "BasketItems");

            migrationBuilder.AlterColumn<int>(
                name: "ClothingColorSizeId",
                table: "BasketItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_ClothingColorSizes_ClothingColorSizeId",
                table: "BasketItems",
                column: "ClothingColorSizeId",
                principalTable: "ClothingColorSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
