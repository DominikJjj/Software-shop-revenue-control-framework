using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace s27400_APBD_Project.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    PESEL = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ClientId", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    KRS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CompanyId", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "Discount",
                columns: table => new
                {
                    DiscountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Offer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("DiscountId", x => x.DiscountId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("RoleId", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "SoftwareCategory",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CategoryId", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    StateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("StateId", x => x.StateId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DueDateRefreshToken = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoleFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("UserId", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleFK",
                        column: x => x.RoleFK,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SoftwareSystem",
                columns: table => new
                {
                    SoftwareId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Version = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    CategoryFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SoftwareId", x => x.SoftwareId);
                    table.ForeignKey(
                        name: "FK_SoftwareSystem_SoftwareCategory_CategoryFK",
                        column: x => x.CategoryFK,
                        principalTable: "SoftwareCategory",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contract",
                columns: table => new
                {
                    ContractId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StateFK = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ContractId", x => x.ContractId);
                    table.ForeignKey(
                        name: "FK_Contract_State_StateFK",
                        column: x => x.StateFK,
                        principalTable: "State",
                        principalColumn: "StateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductDiscount",
                columns: table => new
                {
                    DiscountId = table.Column<int>(type: "int", nullable: false),
                    SoftwareSystemsSoftwareId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDiscount", x => new { x.DiscountId, x.SoftwareSystemsSoftwareId });
                    table.ForeignKey(
                        name: "FK_ProductDiscount_Discount_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discount",
                        principalColumn: "DiscountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductDiscount_SoftwareSystem_SoftwareSystemsSoftwareId",
                        column: x => x.SoftwareSystemsSoftwareId,
                        principalTable: "SoftwareSystem",
                        principalColumn: "SoftwareId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractSoftware",
                columns: table => new
                {
                    ContractSoftwareId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractFK = table.Column<int>(type: "int", nullable: false),
                    SoftwareSystemFK = table.Column<int>(type: "int", nullable: false),
                    UpdateTime = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PriceInContract = table.Column<decimal>(type: "decimal(7,2)", precision: 7, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ContractSoftwareId", x => x.ContractSoftwareId);
                    table.ForeignKey(
                        name: "FK_ContractSoftware_Contract_ContractFK",
                        column: x => x.ContractFK,
                        principalTable: "Contract",
                        principalColumn: "ContractId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractSoftware_SoftwareSystem_SoftwareSystemFK",
                        column: x => x.SoftwareSystemFK,
                        principalTable: "SoftwareSystem",
                        principalColumn: "SoftwareId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientFK = table.Column<int>(type: "int", nullable: true),
                    CompanyFK = table.Column<int>(type: "int", nullable: true),
                    ContractFK = table.Column<int>(type: "int", nullable: false),
                    ValuePaid = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PaymentId", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payment_Client_ClientFK",
                        column: x => x.ClientFK,
                        principalTable: "Client",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payment_Company_CompanyFK",
                        column: x => x.CompanyFK,
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payment_Contract_ContractFK",
                        column: x => x.ContractFK,
                        principalTable: "Contract",
                        principalColumn: "ContractId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "ClientId", "Email", "IsDeleted", "Name", "PESEL", "PhoneNumber", "Surname" },
                values: new object[,]
                {
                    { 1, "anna.kowalska@example.com", false, "Anna", "84051412345", "600123456", "Kowalska" },
                    { 2, "piotr.nowak@example.com", false, "Piotr", "92071823456", "601234567", "Nowak" },
                    { 3, "katarzyna.wisniewska@example.com", false, "Katarzyna", "78090234567", "602345678", "Wiśniewska" },
                    { 4, "marek.zielinski@example.com", false, "Marek", "86031945678", "603456789", "Zieliński" }
                });

            migrationBuilder.InsertData(
                table: "Company",
                columns: new[] { "CompanyId", "Address", "Email", "KRS", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "ul. Miodowa 10, 00-251 Warszawa", "kontakt@techsolutions.pl", "0000456789", "TechSolutions Sp. z o.o.", "221234567" },
                    { 2, "ul. Zielona 15, 30-001 Kraków", "info@greenenergy.pl", "0000567890", "GreenEnergy Polska S.A.", "123456789" },
                    { 3, "ul. Kwiatowa 22, 60-101 Poznań", "biuro@webdesignexperts.pl", "0000678901", "WebDesign Experts Sp. z o.o.", "612345678" },
                    { 4, "ul. Transportowa 5, 40-002 Katowice", "office@smartlogistics.pl", "0000789012", "SmartLogistics S.A.", "324567890" }
                });

            migrationBuilder.InsertData(
                table: "Discount",
                columns: new[] { "DiscountId", "DateEnd", "DateStart", "Name", "Offer", "Value" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wakacje 2023", "Wszystko", 15 },
                    { 2, new DateTime(2024, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wakacje 2024", "Wszystko", 10 },
                    { 3, new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Black Friday 2024", "Wszystko", 20 }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "RoleId", "Name" },
                values: new object[,]
                {
                    { 1, "Standard" },
                    { 2, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "SoftwareCategory",
                columns: new[] { "CategoryId", "Name" },
                values: new object[,]
                {
                    { 1, "Edukacja" },
                    { 2, "Finanse" },
                    { 3, "Rozrywka" }
                });

            migrationBuilder.InsertData(
                table: "State",
                columns: new[] { "StateId", "Name" },
                values: new object[,]
                {
                    { 1, "Oczekujace" },
                    { 2, "Oplacone" },
                    { 3, "Anulowane" }
                });

            migrationBuilder.InsertData(
                table: "Contract",
                columns: new[] { "ContractId", "EndDate", "Price", "StartDate", "StateFK" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 8000.01m, new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 2, new DateTime(2022, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 5000.11m, new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 3, new DateTime(2022, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 9000.11m, new DateTime(2022, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 4, new DateTime(2023, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 4000.01m, new DateTime(2023, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 }
                });

            migrationBuilder.InsertData(
                table: "SoftwareSystem",
                columns: new[] { "SoftwareId", "CategoryFK", "Description", "Name", "Price", "Version" },
                values: new object[,]
                {
                    { 1, 2, "Description Rachunki", "Rachunki", 4000.01m, "1.9" },
                    { 2, 1, "Description Nauczanie", "Nauczanie", 4000.00m, "build 2.20" },
                    { 3, 3, "Description Filmy", "Filmy", 1000.10m, "2.92" }
                });

            migrationBuilder.InsertData(
                table: "ContractSoftware",
                columns: new[] { "ContractSoftwareId", "ContractFK", "PriceInContract", "SoftwareSystemFK", "UpdateTime", "Version" },
                values: new object[,]
                {
                    { 1, 1, 4000.01m, 1, 1, "1.0" },
                    { 2, 1, 4000.00m, 2, 1, "build 2.0" },
                    { 3, 2, 4000.01m, 1, 1, "1.0" },
                    { 4, 2, 1000.10m, 3, 1, "2.21" },
                    { 5, 3, 4000.01m, 1, 1, "1.1" },
                    { 6, 3, 4000.00m, 2, 1, "build 2.1" },
                    { 7, 3, 1000.10m, 3, 1, "2.73" },
                    { 8, 4, 4000.01m, 1, 1, "1.1" }
                });

            migrationBuilder.InsertData(
                table: "Payment",
                columns: new[] { "PaymentId", "ClientFK", "CompanyFK", "ContractFK", "ValuePaid" },
                values: new object[,]
                {
                    { 1, 1, null, 1, 8000.01m },
                    { 2, 3, null, 2, 100.00m },
                    { 3, null, 1, 3, 9000.11m },
                    { 4, null, 2, 4, 400.01m }
                });

            migrationBuilder.InsertData(
                table: "ProductDiscount",
                columns: new[] { "DiscountId", "SoftwareSystemsSoftwareId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 2, 1 },
                    { 2, 2 },
                    { 2, 3 },
                    { 3, 1 },
                    { 3, 2 },
                    { 3, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contract_StateFK",
                table: "Contract",
                column: "StateFK");

            migrationBuilder.CreateIndex(
                name: "IX_ContractSoftware_ContractFK",
                table: "ContractSoftware",
                column: "ContractFK");

            migrationBuilder.CreateIndex(
                name: "IX_ContractSoftware_SoftwareSystemFK",
                table: "ContractSoftware",
                column: "SoftwareSystemFK");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_ClientFK",
                table: "Payment",
                column: "ClientFK");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_CompanyFK",
                table: "Payment",
                column: "CompanyFK");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_ContractFK",
                table: "Payment",
                column: "ContractFK");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDiscount_SoftwareSystemsSoftwareId",
                table: "ProductDiscount",
                column: "SoftwareSystemsSoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareSystem_CategoryFK",
                table: "SoftwareSystem",
                column: "CategoryFK");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleFK",
                table: "User",
                column: "RoleFK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractSoftware");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "ProductDiscount");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "Contract");

            migrationBuilder.DropTable(
                name: "Discount");

            migrationBuilder.DropTable(
                name: "SoftwareSystem");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "State");

            migrationBuilder.DropTable(
                name: "SoftwareCategory");
        }
    }
}
