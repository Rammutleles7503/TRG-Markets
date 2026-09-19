using System.Threading.Tasks;

namespace TRG_Markets.Application.Interfaces
{
    public enum EmergencyControlMode
    {
        Normal,
        ProtectOnly,
        EmergencyStop
    }

    public interface IEmergencyControlService
    {
        EmergencyControlMode CurrentMode { get; }
        Task ActivateEmergencyStopAsync(string reason);
        Task ActivateProtectOnlyModeAsync(string reason);
        Task ResumeNormalTradingAsync(string reason);
    }
}
