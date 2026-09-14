using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TRG_Markets.Domain.Entities;

namespace TRG_Markets.Persistence
{
    public class TRGMarketsDbContext : DbContext
    {
        public TRGMarketsDbContext(DbContextOptions<TRGMarketsDbContext> options) : base(options)
        {
        }

        public DbSet<TradingAccount> TradingAccounts { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Trade> Trades { get; set; } = null!;
        public DbSet<Subscription> Subscriptions { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<Broker> Brokers { get; set; } = null!;
        public DbSet<TradingSessionNotification> TradingSessionNotifications { get; set; } = null!;
        public DbSet<SystemAlert> SystemAlerts { get; set; } = null!;
        public DbSet<MarketHoliday> MarketHolidays { get ; set ; } = null!;
        public DbSet<EquitySnapshot> EquitySnapshots { get ; set ; } = null!;
        public DbSet<ProfitLightAssessment> ProfitLightAssessments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProfitLightAssessment>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => new { x.EaId, x.CalculatedAtUtc });
                entity.Property(x => x.EaId).HasMaxLength(100).IsRequired();
                entity.Property(x => x.Symbol).HasMaxLength(32).IsRequired();
                entity.Property(x => x.Timeframe).HasMaxLength(16).IsRequired();
                entity.Property(x => x.Reasons).HasMaxLength(2000).IsRequired();
                entity.Property(x => x.ModelVersion).HasMaxLength(64).IsRequired();
                entity.Property(x => x.ExpectedValueAfterCosts).HasPrecision(18, 4);
            });
        }
    }
}
