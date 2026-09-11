using System;
using System.Collections.Generic;
using System.Text;
using TRG_Markets.Domain.Entities;
using System.Threading.Tasks;

namespace TRG_Markets.Application.Interfaces
{
    public interface ITradeService
    {
        Task<IEnumerable<Trade>> GetAllTradesAsync();
        Task<Trade?> GetTradeByIdAsync(int id);
        Task<Trade> CreateTradeAsync(Trade trade);

        Task<Trade?> UpdateTradeAsync(int id, Trade trade);

        Task<bool> DeleteTradeAsync(int id);
    }
}
