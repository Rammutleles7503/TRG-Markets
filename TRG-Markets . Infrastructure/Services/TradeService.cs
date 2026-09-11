using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TRG_Markets.Application.Interfaces;
using TRG_Markets.Domain.Entities;
using TRG_Markets.Persistence;

namespace TRG_Markets.Infrastructure.Services
{
    public class TradeService : ITradeService
    {
        private readonly TRGMarketsDbContext  _dbContext ;
        public TradeService(TRGMarketsDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<Trade>> GetAllTradesAsync()
        {
            return await _dbContext.Trades.ToListAsync();
        }

        public async Task<Trade?> GetTradeByIdAsync(int id)
        {
            return await _dbContext.Trades.FindAsync(id);
        }

        public async Task<Trade> CreateTradeAsync(Trade trade)
        {
            _dbContext.Trades.Add(trade);
            await _dbContext.SaveChangesAsync();
            return trade;
        }

        public async Task<Trade?> UpdateTradeAsync(int id, Trade trade)
        {
            if (trade == null) throw new ArgumentNullException(nameof(trade));

            var existing = await _dbContext.Trades.FindAsync(id);
            if (existing == null) return null;

            existing.TradingAccountId = trade.TradingAccountId;
            existing.Symbol = trade.Symbol;
            existing.LotSize = trade.LotSize;
            existing.OpenPrice = trade.OpenPrice;
            existing.ClosePrice = trade.ClosePrice;
            existing.Profit = trade.Profit;
            existing.Comment = trade.Comment;
            existing.TicketNumber = trade.TicketNumber;
            existing.MagicNumber = trade.MagicNumber;
            existing.Volume = trade.Volume;
            existing.BrokerName = trade.BrokerName;
            existing.AccountNumber = trade.AccountNumber;
            existing.ServerName = trade.ServerName;
            existing.BrokerServer = trade.BrokerServer;

            _dbContext.Trades.Update(existing);
            await _dbContext.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteTradeAsync(int id)
        {
            var existing = await _dbContext.Trades.FindAsync(id);
            if (existing == null) return false;

            _dbContext.Trades.Remove(existing);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
