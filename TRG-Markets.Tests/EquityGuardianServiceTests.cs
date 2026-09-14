using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TRG_Markets.Domain.Entities;
using TRG_Markets.Infrastructure.Services;
using TRG_Markets.Persistence;
using Xunit;

namespace TRG_Markets.Tests
{
    public class EquityGuardianServiceTests
    {
        private static TRGMarketsDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<TRGMarketsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TRGMarketsDbContext(options);
        }

        [Fact]
        public async Task RecordSnapshot_WithNoDrawdown_KeepsTradingActive()
        {
            await using var dbContext = CreateDbContext();
            var service = new EquityGuardianService(dbContext);

            var snapshot = new EquitySnapshot
            {
                TradingAccountId = 1,
                Balance = 2000m,
                Equity = 2000m
            };

            var result = await service.RecordSnapshotAsync(snapshot);

            Assert.False(result.TradingSuspended);
            Assert.Equal(0m, result.DrawdownAmount);
            Assert.Equal(0m, result.DrawdownPercentage);
            Assert.Equal(2000m, result.PeakEquity);
        }

        [Fact]
        public async Task RecordSnapshot_WithFifteenPercentDrawdown_SuspendsTrading()
        {
            await using var dbContext = CreateDbContext();
            var service = new EquityGuardianService(dbContext);

            await service.RecordSnapshotAsync(new EquitySnapshot
            {
                TradingAccountId = 1,
                Balance = 2000m,
                Equity = 2000m
            });

            var result = await service.RecordSnapshotAsync(new EquitySnapshot
            {
                TradingAccountId = 1,
                Balance = 2000m,
                Equity = 1700m,
                FloatingProfitLoss = -300m
            });

            Assert.True(result.TradingSuspended);
            Assert.Equal(300m, result.DrawdownAmount);
            Assert.Equal(15m, result.DrawdownPercentage);
            Assert.Equal("Maximum drawdown limit reached.", result.ProtectionReason);
        }

        [Fact]
        public async Task RecordSnapshot_WithTenPercentDrawdown_DoesNotSuspendTrading()
        {
            await using var dbContext = CreateDbContext();
            var service = new EquityGuardianService(dbContext);

            await service.RecordSnapshotAsync(new EquitySnapshot
            {
                TradingAccountId = 2,
                Balance = 2000m,
                Equity = 2000m
            });

            var result = await service.RecordSnapshotAsync(new EquitySnapshot
            {
                TradingAccountId = 2,
                Balance = 2000m,
                Equity = 1800m,
                FloatingProfitLoss = -200m
            });

            Assert.False(result.TradingSuspended);
            Assert.Equal(200m, result.DrawdownAmount);
            Assert.Equal(10m, result.DrawdownPercentage);
        }
       
    }

}
