using System.Threading.Tasks;
using TRG_Markets.Application.Interfaces;

namespace TRG_Markets.Infrastructure.Services
{
    public class EntryAuthorityService : IEntryAuthorityService
    {
        private readonly IEquityGuardianService _equityGuardian;
        private readonly IEmergencyControlService _emergencyControl;

        public EntryAuthorityService(IEquityGuardianService equityGuardian, IEmergencyControlService emergencyControl)
        {
            _equityGuardian = equityGuardian;
            _emergencyControl = emergencyControl;
        }

        public async Task<bool> ShouldAllowEntryAsync(int tradingAccountId)
        {
            if (tradingAccountId <= 0)
                return false;

            // If emergency control is not in Normal mode, disallow new entries
            if (_emergencyControl.CurrentMode != EmergencyControlMode.Normal)
                return false;

            var snapshot = await _equityGuardian.GetLatestSnapshotAsync(tradingAccountId);
            if (snapshot is null)
                return false;

            var suspended = await _equityGuardian.ShouldSuspendTradingAsync(tradingAccountId);
            return !suspended;
        }
    }
}
