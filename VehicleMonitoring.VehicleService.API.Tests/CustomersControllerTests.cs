using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using VehicleMonitoring.VehicleService.API.Controllers;
using VehicleMonitoring.VehicleService.DTO;
using VehicleMonitoring.VehicleService.Infrastructure.UnitOfWork;

namespace VehicleMonitoring.VehicleService.API.Tests
{
    [TestClass]
    public class CustomersControllerTests : TestsBase
    {
        #region Data Members
        private static List<LookupDTO> _customersLookups;
        private static ILogger<CustomersController> _mockLogger;
        private static CustomersController _controller;
        #endregion
        #region Test Attributes
        [ClassInitialize]
        public static void BeforeAllTests(TestContext context)
        {
            _mockLogger = Mock.Of<ILogger<CustomersController>>();
            FillCustomersLookupsListSample();
        }
        [TestInitialize]
        public void BeforeEachTest()
        {
            _mockUOW = new Mock<IVehicleServiceUOW>();
        }
        #endregion
        #region Test Methods
        [TestMethod]
        public void Get_ReturnsJsonResult_WithListOfLookupDTOs()
        {
            // Arrange
            _mockUOW.Setup(uow => uow.GetCustomersLookup()).Returns(_customersLookups);
            _controller = new CustomersController(_mockUOW.Object, _mockLogger, _mockConfigOptions);

            // Act
            var result = _controller.Get();

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            Assert.IsInstanceOfType(result.Value, typeof(IEnumerable<LookupDTO>));
            var modelList = result.Value as List<LookupDTO>;
            Assert.AreEqual(3, modelList.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Ops! we can't process your request currently. Please try again later.")]
        public void Get_ReturnsNiceErrorMessage_WithInternalExceptionOccures()
        {
            // Arrange
            _mockUOW.Setup(uow => uow.GetCustomersLookup()).Throws<Exception>();
            _controller = new CustomersController(_mockUOW.Object, _mockLogger, _mockConfigOptions);

            // Act
            _controller.Get();
        }
        #endregion
        #region Helper Methods
        private static void FillCustomersLookupsListSample()
        {
            _customersLookups = new List<LookupDTO>()
            {
                new LookupDTO(Guid.Parse("EFB499FA-B179-4B99-9539-6925751F1FB6"), "Kalles Grustransporter AB"),
                new LookupDTO(Guid.Parse("A0860071-B1B8-4663-AAD6-6D75A6C92D47"), "Johans Bulk AB"),
                new LookupDTO(Guid.Parse("D47CEC83-BCE8-46ED-B77D-D33D457319F7"), "Haralds Värdetransporter AB")
            };
        }

        #endregion
    }
}
