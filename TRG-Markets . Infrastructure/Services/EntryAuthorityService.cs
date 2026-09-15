using System.Threading.Tasks;
using TRG_Markets.Application.Interfaces;

namespace TRG_Markets.Infrastructure.Services
{
    public class EntryAuthorityService : IEntryAuthorityService
    {
        private readonly IEquityGuardianService _equityGuardian;

        public EntryAuthorityService(IEquityGuardianService equityGuardian)
        {
            _equityGuardian = equityGuardian;
        }

        public async Task<bool> ShouldAllowEntryAsync(int tradingAccountId)
        {
            if (tradingAccountId <= 0)
                return false;

            var snapshot = await _equityGuardian.GetLatestSnapshotAsync(tradingAccountId);
            if (snapshot is null)
                return false;

            var suspended = await _equityGuardian.ShouldSuspendTradingAsync(tradingAccountId);
            return !suspended;
        }
    }
}
