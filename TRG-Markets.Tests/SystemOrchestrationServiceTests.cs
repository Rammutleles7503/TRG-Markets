using System.Threading.Tasks;
using Moq;
using TRG_Markets.Application.Interfaces;
using TRG_Markets.Application.ProfitLight;
using TRG_Markets.Domain.Entities;
using TRG_Markets.Infrastructure.Services;
using Xunit;

namespace TRG_Markets.Tests
{
    public class SystemOrchestrationServiceTests
    {
        [Fact]
        public async Task EvaluateSystemAsync_WhenEquityGuardianSuspends_ReturnsTradingSuspended()
        {
            var entryAuthority = new Mock<IEntryAuthorityService>();
            var profitLight = new Mock<IProfitLightService>();
            var equityGuardian = new Mock<IEquityGuardianService>();
            var systemAlerts = new Mock<ISystemAlertService>();

            equityGuardian.Setup(x => x.ShouldSuspendTradingAsync(1)).ReturnsAsync(true);

            var service = new SystemOrchestrationService(entryAuthority.Object, profitLight.Object, equityGuardian.Object, systemAlerts.Object);

            var result = await service.EvaluateSystemAsync(1);

            Assert.Equal("TRG_Markets system decision: TRADING SUSPENDED BY EQUITY GUARDIAN", result);
            systemAlerts.Verify(x => x.CreateAlertAsync(It.IsAny<SystemAlert>()), Times.Once);
        }

        [Fact]
        public async Task EvaluateSystemAsync_WhenEntryAuthorityBlocks_ReturnsEntryBlocked()
        {
            var entryAuthority = new Mock<IEntryAuthorityService>();
            var profitLight = new Mock<IProfitLightService>();
            var equityGuardian = new Mock<IEquityGuardianService>();
            var systemAlerts = new Mock<ISystemAlertService>();

            equityGuardian.Setup(x => x.ShouldSuspendTradingAsync(1)).ReturnsAsync(false);
            entryAuthority.Setup(x => x.ShouldAllowEntryAsync(1)).ReturnsAsync(false);

            var service = new SystemOrchestrationService(entryAuthority.Object, profitLight.Object, equityGuardian.Object, systemAlerts.Object);

            var result = await service.EvaluateSystemAsync(1);

            Assert.Equal("TRG_Markets system decision: ENTRY BLOCKED", result);
            systemAlerts.Verify(x => x.CreateAlertAsync(It.IsAny<SystemAlert>()), Times.Once);
        }

        [Fact]
        public async Task EvaluateSystemAsync_WhenAllChecksPass_ReturnsEntryApproved()
        {
            var entryAuthority = new Mock<IEntryAuthorityService>();
            var profitLight = new Mock<IProfitLightService>();
            var equityGuardian = new Mock<IEquityGuardianService>();
            var systemAlerts = new Mock<ISystemAlertService>();

            equityGuardian.Setup(x => x.ShouldSuspendTradingAsync(1)).ReturnsAsync(false);
            entryAuthority.Setup(x => x.ShouldAllowEntryAsync(1)).ReturnsAsync(true);

            var service = new SystemOrchestrationService(entryAuthority.Object, profitLight.Object, equityGuardian.Object, systemAlerts.Object);

            var result = await service.EvaluateSystemAsync(1);

            Assert.Equal("TRG_Markets system decision: ENTRY APPROVED", result);
            systemAlerts.Verify(x => x.CreateAlertAsync(It.IsAny<SystemAlert>()), Times.Never);
        }
    }
}
