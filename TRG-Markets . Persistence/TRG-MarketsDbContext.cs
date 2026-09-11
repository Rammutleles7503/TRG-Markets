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
        public DbSet<MarketHoliday> MarketHolidays { get; set; } = null!;
    }
}
