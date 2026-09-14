using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TRG_Markets.Application.Interfaces;
using TRG_Markets.Domain.Entities;
using TRG_Markets.Persistence;

namespace TRG_Markets.Infrastructure.Services
{
    public class EquityGuardianService : IEquityGuardianService
    {
        private readonly TRGMarketsDbContext _dbContext;
        private readonly INotificationService? _notificationService;
        private readonly ISystemAlertService? _systemAlertService;

        public EquityGuardianService(TRGMarketsDbContext dbContext, INotificationService? notificationService = null, ISystemAlertService? systemAlertService = null)
        {
            _dbContext = dbContext;
            _notificationService = notificationService;
            _systemAlertService = systemAlertService;
        }
        public async Task<EquitySnapshot> RecordSnapshotAsync(EquitySnapshot snapshot)
        {
            var wasAlreadySuspended = await _dbContext.EquitySnapshots
                .Where(x => x.TradingAccountId == snapshot.TradingAccountId)
                .OrderByDescending(x => x.RecordedAtUtc)
                .Select(x => x.TradingSuspended)
                .FirstOrDefaultAsync();

            var previousPeak = await _dbContext.EquitySnapshots
                .Where(x => x.TradingAccountId == snapshot.TradingAccountId)
                .MaxAsync(x => (decimal?)x.PeakEquity) ?? snapshot.Equity;

            snapshot.PeakEquity = Math.Max(previousPeak, snapshot.Equity);
            snapshot.DrawdownAmount = Math.Max(0m, snapshot.PeakEquity - snapshot.Equity);
            snapshot.DrawdownPercentage = snapshot.PeakEquity > 0
                ? snapshot.DrawdownAmount / snapshot.PeakEquity * 100m
                : 0m;
            // Do not suspend for exactly 10%; suspend only when drawdown strictly exceeds the threshold
            snapshot.TradingSuspended = snapshot.DrawdownPercentage > 10m;
            snapshot.ProtectionReason = snapshot.TradingSuspended ? "Maximum drawdown limit reached." : null;
            snapshot.RecordedAtUtc = DateTime.UtcNow;

            _dbContext.EquitySnapshots.Add(snapshot);
            await _dbContext.SaveChangesAsync();

            if (snapshot.TradingSuspended && !wasAlreadySuspended)
            {
                var alertMessage = $"Trading account {snapshot.TradingAccountId} has been suspended because drawdown reached {snapshot.DrawdownPercentage:F2}%";

                if (_notificationService is not null)
                {
                    await _notificationService.CreateAsync(new Notification
                    {
                        Title = "Equity Guardian Suspension",
                        Message = alertMessage,
                        Type = "Risk",
                        Severity = "Critical",
                        IsRead = false,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                if (_systemAlertService is not null)
                {
                    await _systemAlertService.CreateAlertAsync(new SystemAlert
                    {
                        Title = "Equity Guardian Suspension",
                        Message = alertMessage,
                        Severity = "Critical",
                        IsRead = false,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            // If the account was previously suspended but is no longer suspended, emit recovery notifications
            if (wasAlreadySuspended && !snapshot.TradingSuspended)
            {
                var recoveryMessage = $"Trading account {snapshot.TradingAccountId} recovered below the 10.00% maximum drawdown threshold. Current drawdown is {snapshot.DrawdownPercentage:F2}%.";

                if (_notificationService is not null)
                {
                    await _notificationService.CreateAsync(new Notification
                    {
                        Title = "Equity Guardian Recovery",
                        Message = recoveryMessage,
                        Type = "Risk",
                        Severity = "Information",
                        IsRead = false,
                        IsArchived = false,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                if (_systemAlertService is not null)
                {
                    await _systemAlertService.CreateAlertAsync(new SystemAlert
                    {
                        Title = "Equity Guardian Recovery",
                        Message = recoveryMessage,
                        Severity = "Info",
                        IsRead = false,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            return snapshot;
        }

        public async Task<EquitySnapshot?> GetLatestSnapshotAsync(int tradingAccountId)
        {
            return await _dbContext.EquitySnapshots.AsNoTracking()
                .Where(x => x.TradingAccountId == tradingAccountId)
                .OrderByDescending(x => x.RecordedAtUtc)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<EquitySnapshot>> GetHistoryAsync(int tradingAccountId, DateTime fromUtc, DateTime toUtc)
        {
            var list = await _dbContext.EquitySnapshots.AsNoTracking()
                .Where(x => x.TradingAccountId == tradingAccountId && x.RecordedAtUtc >= fromUtc && x.RecordedAtUtc <= toUtc)
                .OrderBy(x => x.RecordedAtUtc)
                .ToListAsync();

            return list;
        }

        public async Task<bool> ShouldSuspendTradingAsync(int tradingAccountId)
        {
            var latest = await GetLatestSnapshotAsync(tradingAccountId);
            return latest?.TradingSuspended ?? false;
        }
    }
}
