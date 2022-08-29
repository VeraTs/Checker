using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckerServer.Migrations
{
    public partial class possFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lines_Restaurants_RestaurantID",
                table: "Lines");

            migrationBuilder.RenameColumn(
                name: "RestaurantID",
                table: "Lines",
                newName: "RestaurantId");

            migrationBuilder.RenameIndex(
                name: "IX_Lines_RestaurantID",
                table: "Lines",
                newName: "IX_Lines_RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lines_Restaurants_RestaurantId",
                table: "Lines",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lines_Restaurants_RestaurantId",
                table: "Lines");

            migrationBuilder.RenameColumn(
                name: "RestaurantId",
                table: "Lines",
                newName: "RestaurantID");

            migrationBuilder.RenameIndex(
                name: "IX_Lines_RestaurantId",
                table: "Lines",
                newName: "IX_Lines_RestaurantID");

            migrationBuilder.AddForeignKey(
                name: "FK_Lines_Restaurants_RestaurantID",
                table: "Lines",
                column: "RestaurantID",
                principalTable: "Restaurants",
                principalColumn: "ID");
        }
    }
}
