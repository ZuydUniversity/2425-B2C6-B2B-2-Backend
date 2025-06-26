using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderModelVariable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductionlineID",
                table: "PurchaseOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_ProductionlineID",
                table: "PurchaseOrders",
                column: "ProductionlineID");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_ProductionLines_ProductionlineID",
                table: "PurchaseOrders",
                column: "ProductionlineID",
                principalTable: "ProductionLines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_ProductionLines_ProductionlineID",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_ProductionlineID",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ProductionlineID",
                table: "PurchaseOrders");
        }
    }
}
