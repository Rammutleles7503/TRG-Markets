using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TRG_Markets.Application.Interfaces;
using TRG_Markets.Domain.Entities;
using TRG_Markets.Persistence;

namespace TRG_Markets.Infrastructure.Services
{
    public  class TradingAccountService : ITradingAccountService
    {
        private readonly TRGMarketsDbContext _dbContext;
        // constructor
        public TradingAccountService(TRGMarketsDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<TradingAccount>> GetAllTradingAccountsAsync()
        {
            return await _dbContext.Set<TradingAccount>().ToListAsync();
        }

        public async Task<TradingAccount?> GetTradingAccountByIdAsync(int id)
        {
            return await _dbContext.Set<TradingAccount>().FindAsync(id);
        }

        public async Task<TradingAccount> CreateTradingAccountAsync(TradingAccount tradingAccount)
        {
            if (tradingAccount == null) throw new ArgumentNullException(nameof(tradingAccount));

            await _dbContext.Set<TradingAccount>().AddAsync(tradingAccount);
            await _dbContext.SaveChangesAsync();
            return tradingAccount;
        }

        public async Task<TradingAccount?> UpdateTradingAccountAsync(int id, TradingAccount tradingAccount)
        {
            if (tradingAccount == null) throw new ArgumentNullException(nameof(tradingAccount));

            var existing = await _dbContext.Set<TradingAccount>().FindAsync(id);
            if (existing == null) return null;

            // Update allowed properties; do not change Id or CreatedAt
            existing.AccountNumber = tradingAccount.AccountNumber;
            existing.Broker = tradingAccount.Broker;
            existing.Balance = tradingAccount.Balance;
            existing.Equity = tradingAccount.Equity;
            existing.FreeMargin = tradingAccount.FreeMargin;
            existing.Margin = tradingAccount.Margin;
            existing.Drawdown = tradingAccount.Drawdown;
            existing.Platform = tradingAccount.Platform;
            existing.ServerName = tradingAccount.ServerName;
            existing.AccountOwner = tradingAccount.AccountOwner;
            existing.IsActive = tradingAccount.IsActive;
            existing.AccountType = tradingAccount.AccountType;
            existing.Currency = tradingAccount.Currency;

            _dbContext.Set<TradingAccount>().Update(existing);
            await _dbContext.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteTradingAccountAsync(int id)
        {
            var existing = await _dbContext.Set<TradingAccount>().FindAsync(id);
            if (existing == null) return false;

            _dbContext.Set<TradingAccount>().Remove(existing);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
