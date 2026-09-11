using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TRG_Markets.Application.Interfaces;
using TRG_Markets.Domain.Entities;
using TRG_Markets.Persistence;

namespace TRG_Markets.Infrastructure.Services
{
    public class SystemAlertService : ISystemAlertService
    {
        private readonly TRGMarketsDbContext _context;

        public SystemAlertService(TRGMarketsDbContext context)
        {
            _context = context;
        }

        public async Task<SystemAlert?> GetAlertAsync(int id)
        {
            return await _context.SystemAlerts.FindAsync(id);
        }

        public async Task<SystemAlert> CreateAlertAsync(SystemAlert alert)
        {
            await _context.SystemAlerts.AddAsync(alert);
            await _context.SaveChangesAsync();
            return alert;
        }

        public async Task<List<SystemAlert>> GetAllAlertsAsync()
        {
            return await _context.SystemAlerts.ToListAsync();
        }

        public async Task<SystemAlert?> UpdateAlertAsync(int id, SystemAlert alert)
        {
            var existing = await _context.SystemAlerts.FindAsync(id);
            if (existing == null)
            {
                return null;
            }

            existing.Title = alert.Title;
            existing.Message = alert.Message;
            existing.Severity = alert.Severity;
            existing.IsRead = alert.IsRead;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAlertAsync(int id)
        {
            var existing = await _context.SystemAlerts.FindAsync(id);
            if (existing == null)
            {
                return false;
            }

            _context.SystemAlerts.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
