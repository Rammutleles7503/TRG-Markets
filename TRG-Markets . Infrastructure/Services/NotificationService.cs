using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRG_Markets.Application.Interfaces;
using TRG_Markets.Domain.Entities;
using TRG_Markets.Persistence;

namespace TRG_Markets.Infrastructure.Services
{
    //private readonly TRGMarketsDbContext_dbContext;
    public class NotificationService : INotificationService
    {
        private readonly TRGMarketsDbContext _dbContext;

        public NotificationService(TRGMarketsDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Notification> CreateAsync(Notification notification)
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification));
            await _dbContext.Notifications.AddAsync(notification);
            await _dbContext.SaveChangesAsync();
            return notification;
        }


        public async Task<List<Notification>> GetALLAsync()
        {
            return await _dbContext.Notifications.ToListAsync().ConfigureAwait(false);
        }

        public async Task<Notification?> GetByIdAsync(int id)
        {
            return await _dbContext.Notifications.FindAsync(id).ConfigureAwait(false);
        }

        public Task<List<Notification>> GetUnreadAsync()
        {
            return _dbContext.Notifications.Where(n => !n.IsRead).ToListAsync();
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            var notification = await _dbContext.Notifications.FindAsync(id).ConfigureAwait(false);
            if (notification == null)
            {
                return false;
            }

            if (notification.IsRead)
            {
                return true;
            }

            notification.IsRead = true;
            var changes = await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            return changes > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var notification = await _dbContext.Notifications.FindAsync(id).ConfigureAwait(false);
            if (notification == null)
            {
                return false;
            }

            _dbContext.Notifications.Remove(notification);
            var changes = await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            return changes > 0;
        }
    }
}
