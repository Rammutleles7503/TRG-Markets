using TRG_Markets.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TRG_Markets.Application.Interfaces
{
    public interface ITradingAccountService
    {
        // 1. Get all trading accounts
        Task<IEnumerable<TradingAccount>> GetAllTradingAccountsAsync();

        // 2. Get a trading account by id
        Task<TradingAccount?> GetTradingAccountByIdAsync(int id);

        // 3. Create a new trading account
        Task<TradingAccount> CreateTradingAccountAsync(TradingAccount tradingAccount);

        // 4. Update a trading account
        Task<TradingAccount?> UpdateTradingAccountAsync(int id, TradingAccount tradingAccount);

        // 5. Delete a trading account
        Task<bool> DeleteTradingAccountAsync(int id);
    }
}
