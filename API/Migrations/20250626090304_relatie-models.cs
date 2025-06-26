using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class relatiemodels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ProductionLines",
                newName: "ProductionLineId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PartsDelivery",
                newName: "PartsDeliveryId");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "PartsDelivery",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccountManagerId",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductionLineId",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Expeditions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AccountManager",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountManager", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PartsDelivery_OrderId",
                table: "PartsDelivery",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_AccountManagerId",
                table: "Order",
                column: "AccountManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ProductionLineId",
                table: "Order",
                column: "ProductionLineId");

            migrationBuilder.CreateIndex(
                name: "IX_Expeditions_OrderId",
                table: "Expeditions",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expeditions_Order_OrderId",
                table: "Expeditions",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AccountManager_AccountManagerId",
                table: "Order",
                column: "AccountManagerId",
                principalTable: "AccountManager",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_ProductionLines_ProductionLineId",
                table: "Order",
                column: "ProductionLineId",
                principalTable: "ProductionLines",
                principalColumn: "ProductionLineId");

            migrationBuilder.AddForeignKey(
                name: "FK_PartsDelivery_Order_OrderId",
                table: "PartsDelivery",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expeditions_Order_OrderId",
                table: "Expeditions");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_AccountManager_AccountManagerId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_ProductionLines_ProductionLineId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_PartsDelivery_Order_OrderId",
                table: "PartsDelivery");

            migrationBuilder.DropTable(
                name: "AccountManager");

            migrationBuilder.DropIndex(
                name: "IX_PartsDelivery_OrderId",
                table: "PartsDelivery");

            migrationBuilder.DropIndex(
                name: "IX_Order_AccountManagerId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_ProductionLineId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Expeditions_OrderId",
                table: "Expeditions");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "PartsDelivery");

            migrationBuilder.DropColumn(
                name: "AccountManagerId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ProductionLineId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Expeditions");

            migrationBuilder.RenameColumn(
                name: "ProductionLineId",
                table: "ProductionLines",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PartsDeliveryId",
                table: "PartsDelivery",
                newName: "Id");
        }
    }
}
