using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRG_Markets.Domain.Entities;

namespace TRG_Markets.Application.Interfaces
{
    public interface IEquityGuardianService
    {
        Task<EquitySnapshot> RecordSnapshotAsync(EquitySnapshot snapshot);
        Task<EquitySnapshot?> GetLatestSnapshotAsync(int tradingAccountId);
        Task<IReadOnlyList<EquitySnapshot>> GetHistoryAsync(int tradingAccountId, DateTime fromUtc, DateTime toUtc);
        Task<bool> ShouldSuspendTradingAsync(int tradingAccountId);
    }
}
