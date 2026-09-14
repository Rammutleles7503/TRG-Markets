using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TRG_Markets_._Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEquitySnapshotFoundation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquitySnapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TradingAccountId = table.Column<int>(type: "int", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Equity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FloatingProfitLoss = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DailyProfitLoss = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PeakEquity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DrawdownAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DrawdownPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OpenPositions = table.Column<int>(type: "int", nullable: false),
                    TradingSuspended = table.Column<bool>(type: "bit", nullable: false),
                    ProtectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecordedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquitySnapshots", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquitySnapshots");
        }
    }
}
