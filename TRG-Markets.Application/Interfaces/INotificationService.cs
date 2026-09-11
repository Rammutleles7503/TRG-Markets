using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TRG_Markets.Domain.Entities;

namespace TRG_Markets.Application.Interfaces
{
    public interface INotificationService
        
    {
        Task<List<Notification>> GetALLAsync();
        Task<List<Notification>> GetUnreadAsync();
        Task<Notification?> GetByIdAsync(int id);
        Task<Notification> CreateAsync(Notification notification);
        Task<bool> MarkAsReadAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
