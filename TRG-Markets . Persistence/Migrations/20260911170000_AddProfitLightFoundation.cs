using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TRG_Markets.Persistence;

#nullable disable

namespace TRG_Markets_._Persistence.Migrations;

[DbContext(typeof(TRGMarketsDbContext))]
[Migration("20260911170000_AddProfitLightFoundation")]
public partial class AddProfitLightFoundation : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "ProfitLightAssessments",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                EaId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                MagicNumber = table.Column<long>(type: "bigint", nullable: false),
                AccountId = table.Column<int>(type: "int", nullable: false),
                Symbol = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                Timeframe = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                Score = table.Column<int>(type: "int", nullable: true),
                LightState = table.Column<int>(type: "int", nullable: false),
                RecommendedAction = table.Column<int>(type: "int", nullable: false),
                Confidence = table.Column<int>(type: "int", nullable: false),
                SafetyDecision = table.Column<int>(type: "int", nullable: false),
                ExpectedValueAfterCosts = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                Reasons = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                ModelVersion = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                CalculatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                ExpiresAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_ProfitLightAssessments", x => x.Id));

        migrationBuilder.CreateIndex(
            name: "IX_ProfitLightAssessments_EaId_CalculatedAtUtc",
            table: "ProfitLightAssessments",
            columns: new[] { "EaId", "CalculatedAtUtc" });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
        => migrationBuilder.DropTable(name: "ProfitLightAssessments");
}
