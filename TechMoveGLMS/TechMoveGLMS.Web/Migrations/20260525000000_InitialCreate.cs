using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechMoveGLMS.Web.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Clients",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                ContactDetails = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                Region = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Clients", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Contracts",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ClientId = table.Column<int>(type: "int", nullable: false),
                StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false),
                ServiceLevel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                SignedAgreementFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SignedAgreementPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Contracts", x => x.Id);
                table.ForeignKey(
                    name: "FK_Contracts_Clients_ClientId",
                    column: x => x.ClientId,
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "ServiceRequests",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ContractId = table.Column<int>(type: "int", nullable: false),
                Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                CostUsd = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                ExchangeRateUsdToZar = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                CostZar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ServiceRequests", x => x.Id);
                table.ForeignKey(
                    name: "FK_ServiceRequests_Contracts_ContractId",
                    column: x => x.ContractId,
                    principalTable: "Contracts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.InsertData(
            table: "Clients",
            columns: new[] { "Id", "ContactDetails", "Name", "Region" },
            values: new object[,]
            {
                { 1, "operations@northwind.example | +27 11 555 0101", "Northwind Retail Group", "Gauteng" },
                { 2, "logistics@capeexport.example | +27 21 555 0188", "Cape Export Traders", "Western Cape" }
            });

        migrationBuilder.InsertData(
            table: "Contracts",
            columns: new[] { "Id", "ClientId", "EndDate", "ServiceLevel", "SignedAgreementFileName", "SignedAgreementPath", "StartDate", "Status" },
            values: new object[,]
            {
                { 1, 1, new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Priority Freight", null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                { 2, 1, new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Warehousing", null, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                { 3, 2, new DateTime(2027, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customs Clearance", null, null, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 }
            });

        migrationBuilder.InsertData(
            table: "ServiceRequests",
            columns: new[] { "Id", "ContractId", "CostUsd", "CostZar", "CreatedAt", "Description", "ExchangeRateUsdToZar", "Status" },
            values: new object[,]
            {
                { 1, 1, 1000m, 18500m, new DateTime(2026, 4, 1, 8, 30, 0, 0, DateTimeKind.Utc), "Urgent Johannesburg to Durban freight movement.", 18.50m, 1 },
                { 2, 1, 750m, 13875m, new DateTime(2026, 4, 15, 10, 0, 0, 0, DateTimeKind.Utc), "Temporary bonded warehouse handling for imported stock.", 18.50m, 0 }
            });

        migrationBuilder.CreateIndex(
            name: "IX_Contracts_ClientId",
            table: "Contracts",
            column: "ClientId");

        migrationBuilder.CreateIndex(
            name: "IX_ServiceRequests_ContractId",
            table: "ServiceRequests",
            column: "ContractId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "ServiceRequests");
        migrationBuilder.DropTable(name: "Contracts");
        migrationBuilder.DropTable(name: "Clients");
    }
}
