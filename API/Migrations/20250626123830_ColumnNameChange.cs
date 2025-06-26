using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class ColumnNameChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_ProductionLines_ProductionlineID",
                table: "PurchaseOrders");

            migrationBuilder.RenameColumn(
                name: "ProductionlineID",
                table: "PurchaseOrders",
                newName: "ProductionLineId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseOrders_ProductionlineID",
                table: "PurchaseOrders",
                newName: "IX_PurchaseOrders_ProductionLineId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_ProductionLines_ProductionLineId",
                table: "PurchaseOrders",
                column: "ProductionLineId",
                principalTable: "ProductionLines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_ProductionLines_ProductionLineId",
                table: "PurchaseOrders");

            migrationBuilder.RenameColumn(
                name: "ProductionLineId",
                table: "PurchaseOrders",
                newName: "ProductionlineID");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseOrders_ProductionLineId",
                table: "PurchaseOrders",
                newName: "IX_PurchaseOrders_ProductionlineID");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_ProductionLines_ProductionlineID",
                table: "PurchaseOrders",
                column: "ProductionlineID",
                principalTable: "ProductionLines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
