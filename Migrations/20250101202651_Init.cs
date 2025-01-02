using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Authentication_Client.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chambre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    PrixParNuit = table.Column<double>(type: "float", nullable: false),
                    Disponibilite = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chambre", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    CIN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prix = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Etat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ChambreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservation_Chambre_ChambreId",
                        column: x => x.ChambreId,
                        principalTable: "Chambre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservation_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Facture",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MontantTotal = table.Column<double>(type: "float", nullable: false),
                    DatePaiement = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Etat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReservationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facture", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Facture_Reservation_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Chambre",
                columns: new[] { "Id", "Disponibilite", "Numero", "PrixParNuit", "Type" },
                values: new object[,]
                {
                    { 1, true, 101, 200.0, 0 },
                    { 2, true, 102, 100.0, 1 },
                    { 3, true, 103, 150.0, 2 },
                    { 4, true, 104, 110.0, 1 },
                    { 5, true, 105, 250.0, 0 },
                    { 6, true, 106, 140.0, 2 },
                    { 7, true, 107, 120.0, 1 },
                    { 8, true, 108, 230.0, 0 },
                    { 9, true, 109, 160.0, 2 },
                    { 10, true, 110, 105.0, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facture_ReservationId",
                table: "Facture",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_ChambreId",
                table: "Reservation",
                column: "ChambreId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_ClientId",
                table: "Reservation",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Facture");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "Reservation");

            migrationBuilder.DropTable(
                name: "Chambre");

            migrationBuilder.DropTable(
                name: "Client");
        }
    }
}
