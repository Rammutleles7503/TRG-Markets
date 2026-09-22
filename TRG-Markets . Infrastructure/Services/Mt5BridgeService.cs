using System;
using System.Threading;
using System.Threading.Tasks;
using TRG_Markets.Application.Interfaces;

namespace TRG_Markets.Infrastructure.Services
{
    public sealed class Mt5BridgeService : IMt5BridgeService
    {
        public Task<Mt5BridgeStatus> GetStatusAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var status = new Mt5BridgeStatus(
                IsConnected: false,
                TerminalName: "MT5 Bridge - Dry Run",
                Login: null,
                Broker: "Not connected",
                CheckedAtUtc: DateTime.UtcNow,
                Message: "Safe dry-run mode is active. Live trading is disabled.");

            return Task.FromResult(status);
        }

        public Task<Mt5CommandResult> SendCommandAsync(Mt5TradeCommand command, CancellationToken cancellation = default)
        {
            ArgumentNullException.ThrowIfNull(command);
            cancellation.ThrowIfCancellationRequested();

            if (!command.DryRun)
            {
                return Task.FromResult(new Mt5CommandResult(
                    Accepted: false,
                    Status: "Live_EXECUTION_BLOCKED",
                    Message: "Live MT5 execution is not enabled.",
                    ExternalOrderId: null,
                    ProcessedAtUtc: DateTime.UtcNow));
            }

            return Task.FromResult(new Mt5CommandResult(
                Accepted: true,
                Status: "DRY-RUN-ACCEPTED",
                Message: $"Dry-run command accepted for {command.Symbol}. No real trade was placed.",
                ExternalOrderId: null,
                ProcessedAtUtc: DateTime.UtcNow));
        }
    }
}
