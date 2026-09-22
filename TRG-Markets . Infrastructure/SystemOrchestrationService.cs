using System;
using System.Threading.Tasks;
using TRG_Markets.Application.Interfaces;
using TRG_Markets.Application.ProfitLight;
using TRG_Markets.Domain.Entities;

namespace TRG_Markets.Infrastructure.Services
{
    public class SystemOrchestrationService : ISystemOrchestrationService
    {
        private readonly IEntryAuthorityService _entryAuthorityService;
        private readonly IProfitLightService _profitLightService;
        private readonly IEquityGuardianService _equityGuardianService;
        private readonly ISystemAlertService _systemAlertService;

        public SystemOrchestrationService(
            IEntryAuthorityService entryAuthorityService,
            IProfitLightService profitLightService,
            IEquityGuardianService equityGuardianService,
            ISystemAlertService systemAlertService)
        {
            _entryAuthorityService = entryAuthorityService ?? throw new System.ArgumentNullException(nameof(entryAuthorityService));
            _profitLightService = profitLightService ?? throw new System.ArgumentNullException(nameof(profitLightService));
            _equityGuardianService = equityGuardianService ?? throw new System.ArgumentNullException(nameof(equityGuardianService));
            _systemAlertService = systemAlertService ?? throw new System.ArgumentNullException(nameof(systemAlertService));
        }

        public async Task<string> EvaluateSystemAsync(int tradingAccountId)
        {
            var shouldSuspend = await _equityGuardianService.ShouldSuspendTradingAsync(tradingAccountId);
            if (shouldSuspend)
            {
                await _systemAlertService.CreateAlertAsync(new SystemAlert
                {
                    Title = "Trading Suspended",
                    Message = $"Equity Guardian suspended trading for account {tradingAccountId}.",
                    Severity = "Critical",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                });

                return "TRG_Markets system decision: TRADING SUSPENDED BY EQUITY GUARDIAN";
            }

            var entryAllowed = await _entryAuthorityService.ShouldAllowEntryAsync(tradingAccountId);
            if (!entryAllowed)
            {
                await _systemAlertService.CreateAlertAsync(new SystemAlert
                {
                    Title = "Entry Blocked",
                    Message = $"Entry Authority blocked a trade for account {tradingAccountId}.",
                    Severity = "Warng",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                });

                return "TRG_Markets system decision: ENTRY BLOCKED";
            }

            return "TRG_Markets system decision: ENTRY APPROVED";
        }

        public async Task<string> EvaluatePreTradeAsync(ProfitLightPreTradeRequest request)
        {
            var shouldSuspend = await _equityGuardianService.ShouldSuspendTradingAsync(request.AccountId);
            if (shouldSuspend)
            {
                await _systemAlertService.CreateAlertAsync(new SystemAlert
                {
                    Title = "Pre-Trade suspended",
                    Message = $"Equity Guardian suspended pre-trade evaluation for account {request.AccountId}.",
                    Severity = "Critical",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                });

                return "TRG_Markets system decision: TRADING SUSPENDED BY EQUITY GUARDIAN";
            }

            var entryAllowed = await _entryAuthorityService.ShouldAllowEntryAsync(request.AccountId);
            if (!entryAllowed)
            {
                await _systemAlertService.CreateAlertAsync(new SystemAlert
                {
                    Title = "Pre-Trade Entry Blocked",
                    Message = $"Entry Authority blocked pre-trade entry for account {request.AccountId}.",
                    Severity = "Warning",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                });

                return "TRG_Markets system decision: ENTRY BLOCKED BY ENTRY AUTHORITY";
            }

            var profitLight = await _profitLightService.AssessPreTradeAsync(request);
            // profitLight is ProfitLightResult
            if (profitLight.SafetyDecision != ProfitLightSafetyDecision.Allow)
            {
                await _systemAlertService.CreateAlertAsync(new SystemAlert
                {
                    Title = "Profit Light Blocked Entry",
                    Message = $"Profit Light blocked entry for account {request.AccountId}: {profitLight.DisplayLabel}.",
                    Severity = "Warning",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                });

                return $"TRG_Markets system decision: ENTRY BLOCKED BY PROFIT LIGHT - {profitLight.DisplayLabel}";
            }

            if (profitLight.RecommendedAction != ProfitLightAction.Enter)
            {
                await _systemAlertService.CreateAlertAsync(new SystemAlert
                {
                    Title = "Profit Light Wait",
                    Message = $"Profit Light advised waiting for account {request.AccountId}: {profitLight.DisplayLabel}.",
                    Severity = "Info",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                });

                return $"TRG_Markets system decision: WAIT - {profitLight.DisplayLabel}";
            }

            return $"TRG_Markets system decision: ENTRY APPROVED - {profitLight.DisplayLabel}";
        }
    }
}
