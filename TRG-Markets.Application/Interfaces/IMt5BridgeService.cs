using System;
using System.Threading;
using System.Threading.Tasks;

namespace TRG_Markets.Application.Interfaces
{
    public interface IMt5BridgeService
    {
        Task<Mt5BridgeStatus> GetStatusAsync(CancellationToken cancellationToken = default);
        Task<Mt5CommandResult> SendCommandAsync(Mt5TradeCommand command, CancellationToken cancellation = default);
    }

    public sealed record Mt5BridgeStatus(bool IsConnected, string TerminalName, long? Login, string Broker, DateTime CheckedAtUtc, string Message);

    public sealed record Mt5TradeCommand(
        int TradingAccountId,
        string Symbol,
        string Action,
        decimal Volume,
        decimal? StopLoss,
        decimal? TakeProfit,
        long MagicNumber,
        bool DryRun = true);

    public sealed record Mt5CommandResult(bool Accepted, string Status, string Message, string? ExternalOrderId, DateTime ProcessedAtUtc);
}
