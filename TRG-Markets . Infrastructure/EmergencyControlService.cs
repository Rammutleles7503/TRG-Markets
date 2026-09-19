using System.Threading.Tasks;
using TRG_Markets.Application.Interfaces;

namespace TRG_Markets.Infrastructure.Services
{
    public sealed class EmergencyControlService : IEmergencyControlService
    {
        private readonly object _modeLock = new();

        public EmergencyControlMode CurrentMode { get; private set; } = EmergencyControlMode.Normal;

        public Task ActivateEmergencyStopAsync(string reason)
        {
            lock (_modeLock)
            {
                CurrentMode = EmergencyControlMode.EmergencyStop;
            }

            return Task.CompletedTask;
        }

        public Task ActivateProtectOnlyModeAsync(string reason)
        {
            lock (_modeLock)
            {
                CurrentMode = EmergencyControlMode.ProtectOnly;
            }

            return Task.CompletedTask;
        }

        public Task ResumeNormalTradingAsync(string reason)
        {
            lock (_modeLock)
            {
                CurrentMode = EmergencyControlMode.Normal;
            }

            return Task.CompletedTask;
        }
    }
}
