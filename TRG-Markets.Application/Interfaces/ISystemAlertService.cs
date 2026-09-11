using TRG_Markets.Domain.Entities;

namespace TRG_Markets.Application.Interfaces
{
    public interface ISystemAlertService
    {
        Task<SystemAlert?>
            GetAlertAsync(int id);
        Task<SystemAlert>
            CreateAlertAsync(SystemAlert alert);
        Task<List<SystemAlert>> GetAllAlertsAsync();
        Task<SystemAlert?> UpdateAlertAsync(int id, SystemAlert alert);
        Task<bool> DeleteAlertAsync(int id);
    }
}
