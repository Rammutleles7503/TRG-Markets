using System.Threading.Tasks;

namespace TRG_Markets.Application.Interfaces
{
    public interface IEntryAuthorityService
    {
        Task<bool> ShouldAllowEntryAsync(int tradingAccountId);
    }
}
