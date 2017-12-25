using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VehicleMonitoring.VehicleService.Infrastructure.UnitOfWork;

namespace VehicleMonitoring.VehicleService.API.Tests
{
    [TestClass]
    public class TestsBase
    {
        #region Data Members
        protected static IOptions<GeneralAppSettings> _mockConfigOptions;
        protected static Mock<IVehicleServiceUOW> _mockUOW;
        #endregion
        #region Test Attributes
        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            _mockConfigOptions = Mock.Of<IOptions<GeneralAppSettings>>();
        }
        #endregion
    }
}
