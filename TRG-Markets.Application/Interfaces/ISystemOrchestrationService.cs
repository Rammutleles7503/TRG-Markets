using System.Threading.Tasks;
using TRG_Markets.Application.ProfitLight;

namespace TRG_Markets.Application.Interfaces
{
    public interface ISystemOrchestrationService
    {
        Task<string> EvaluateSystemAsync(int tradingAccountId);
        Task<string> EvaluatePreTradeAsync(ProfitLightPreTradeRequest request);
    }
}
