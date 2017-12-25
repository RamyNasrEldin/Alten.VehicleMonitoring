using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using VehicleMonitoring.VehicleService.API.Controllers;
using VehicleMonitoring.VehicleService.DTO;
using VehicleMonitoring.VehicleService.Infrastructure.UnitOfWork;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace VehicleMonitoring.VehicleService.API.Tests
{
    [TestClass]
    public class MonitoringBoardControllerTests : TestsBase
    {
        #region Data Members
        private static ILogger<MonitoringBoardController> _mockLogger;
        private static List<CustomerDTO> _customers;
        private static List<CustomerDTO> _customersWithConnectedVehicles;
        private static MonitoringBoardController _controller;
        #endregion
        #region Test Attributes
        [ClassInitialize]
        public static void BeforeAllTests(TestContext context)
        {
            _mockLogger = Mock.Of<ILogger<MonitoringBoardController>>();
            FillCustomersListSample();
            FillCustomersWithConnectedVehiclesSample();
        }
        [TestInitialize]
        public void BeforeEachTest()
        {
            _mockUOW = new Mock<IVehicleServiceUOW>();
        }
        #endregion
        #region Test Methods
        [TestMethod]
        public void GetCustomersVehicles_ReturnsJsonResult_WithListOfCustomersDTOs()
        {
            // Arrange
            _mockUOW.Setup(uow => uow.GetCustomersVehicles()).Returns(_customers);
            _controller = new MonitoringBoardController(_mockUOW.Object, _mockLogger, _mockConfigOptions);

            // Act
            var result = _controller.GetCustomersVehicles();

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            Assert.IsInstanceOfType(result.Value, typeof(IEnumerable<CustomerDTO>));
            var customersList = result.Value as List<CustomerDTO>;
            Assert.AreEqual(3, customersList.Count);
        }
        [TestMethod]
        public void GetCustomersVehicles_ReturnsCustomers_WithListOfVehiclesDTOsToEachCustomer()
        {
            // Arrange
            _mockUOW.Setup(uow => uow.GetCustomersVehicles()).Returns(_customers);
            _controller = new MonitoringBoardController(_mockUOW.Object, _mockLogger, _mockConfigOptions);

            // Act
            var result = _controller.GetCustomersVehicles();

            // Assert
            var returnedCustomersList = result.Value as List<CustomerDTO>;
            Assert.AreEqual(3, returnedCustomersList[0].Vehicles.Count());
            Assert.AreEqual(2, returnedCustomersList[1].Vehicles.Count());
            Assert.AreEqual(2, returnedCustomersList[2].Vehicles.Count());
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Ops! we can't process your request currently. Please try again later.")]
        public void GetCustomersVehicles_ReturnsNiceErrorMessage_WithInternalExceptionOccures()
        {
            // Arrange
            _mockUOW.Setup(uow => uow.GetCustomersVehicles()).Throws<Exception>();
            _controller = new MonitoringBoardController(_mockUOW.Object, _mockLogger, _mockConfigOptions);

            // Act
            _controller.GetCustomersVehicles();
        }

        [TestMethod]
        public void GetVehiclesByCustomerId_ReturnsJsonResult_WithOneCustomerDTO()
        {
            // Arrange
            var testCustomer = _customers.SingleOrDefault(c => c.CustomerId == new Guid("EFB499FA-B179-4B99-9539-6925751F1FB6"));
            _mockUOW.Setup(uow => uow.GetCustomerVehiclesByCustomerId(new Guid("EFB499FA-B179-4B99-9539-6925751F1FB6"))).Returns(testCustomer);
            _controller = new MonitoringBoardController(_mockUOW.Object, _mockLogger, _mockConfigOptions);

            // Act
            var result = _controller.GetCustomerVehiclesByCustomerId("EFB499FA-B179-4B99-9539-6925751F1FB6");

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            Assert.IsInstanceOfType(result.Value, typeof(CustomerDTO));
            var selectedCustomer = result.Value as CustomerDTO;
            Assert.AreEqual(new Guid("EFB499FA-B179-4B99-9539-6925751F1FB6"), selectedCustomer.CustomerId);
        }

        [TestMethod]
        public void GetVehiclesByCustomerId_ReturnsCustomer_WithListOfVehiclesDTOs()
        {
            // Arrange
            var customerId = new Guid("EFB499FA-B179-4B99-9539-6925751F1FB6");
            var testCustomer = _customers.SingleOrDefault(c => c.CustomerId == customerId);
            _mockUOW.Setup(uow => uow.GetCustomerVehiclesByCustomerId(customerId)).Returns(testCustomer);
            _controller = new MonitoringBoardController(_mockUOW.Object, _mockLogger, _mockConfigOptions);

            // Act
            var result = _controller.GetCustomerVehiclesByCustomerId(customerId.ToString());

            // Assert
            var returnedCustomer = result.Value as CustomerDTO;
            Assert.AreEqual(3, returnedCustomer.Vehicles.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Ops! we can't process your request currently. Please try again later.")]
        public void GetVehiclesByCustomerId_ReturnsNiceErrorMessage_WithInternalExceptionOccures()
        {
            // Arrange
            _mockUOW.Setup(uow => uow.GetCustomerVehiclesByCustomerId(It.IsAny<Guid>())).Throws<Exception>();
            _controller = new MonitoringBoardController(_mockUOW.Object, _mockLogger, _mockConfigOptions);

            // Act
            _controller.GetCustomerVehiclesByCustomerId("EFB499FA-B179-4B99-9539-6925751F1FB6");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Please provide valid customer Id.")]
        public void GetVehiclesByCustomerId_ReturnsNiceErrorMessage_WithInvalidCustomerId()
        {
            // Arrange
           _controller = new MonitoringBoardController(_mockUOW.Object, _mockLogger, _mockConfigOptions);

            // Act
            _controller.GetCustomerVehiclesByCustomerId(null);
        }

        [TestMethod]
        public void GetCustomersVehiclesByStatus_ReturnsJsonResult_WithListOfCustomersDTOs()
        {
            // Arrange
            _mockUOW.Setup(uow => uow.GetCustomersVehiclesByStatus(true)).Returns(_customersWithConnectedVehicles);
            _controller = new MonitoringBoardController(_mockUOW.Object, _mockLogger, _mockConfigOptions);

            // Act
            var result = _controller.GetCustomersVehiclesByStatus(true);

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            Assert.IsInstanceOfType(result.Value, typeof(IEnumerable<CustomerDTO>));
            var customersList = result.Value as List<CustomerDTO>;
            Assert.AreEqual(3, customersList.Count);
        }
        [TestMethod]
        public void GetCustomersVehiclesByStatus_ReturnsCustomers_WithListOfVehiclesDTOsMatchingTheSentStatus()
        {
            // Arrange
            _mockUOW.Setup(uow => uow.GetCustomersVehiclesByStatus(true)).Returns(_customersWithConnectedVehicles);
            _controller = new MonitoringBoardController(_mockUOW.Object, _mockLogger, _mockConfigOptions);

            // Act
            var result = _controller.GetCustomersVehiclesByStatus(true);

            // Assert
            var returnedCustomersList = result.Value as List<CustomerDTO>;
            Assert.AreEqual(2, returnedCustomersList[0].Vehicles.Where(v=>v.CurrentStatus).Count());
            Assert.AreEqual(1, returnedCustomersList[1].Vehicles.Where(v => v.CurrentStatus).Count());
            Assert.AreEqual(1, returnedCustomersList[2].Vehicles.Where(v => v.CurrentStatus).Count());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Ops! we can't process your request currently. Please try again later.")]
        public void GetCustomersVehiclesByStatus_ReturnsNiceErrorMessage_WithInternalExceptionOccures()
        {
            // Arrange
            _mockUOW.Setup(uow => uow.GetCustomersVehiclesByStatus(It.IsAny<bool>())).Throws<Exception>();
            _controller = new MonitoringBoardController(_mockUOW.Object, _mockLogger, _mockConfigOptions);

            // Act
            _controller.GetCustomersVehiclesByStatus(false);
        }

        #endregion
        #region Helper Methods
        private static void FillCustomersListSample()
        {
            _customers = new List<CustomerDTO>();
            var customer1Vehicles = new List<VehicleDTO>();
            customer1Vehicles.AddRange(new List<VehicleDTO>() {
                new VehicleDTO("YS2R4X20005399401","ABC123",Guid.Parse("EFB499FA-B179-4B99-9539-6925751F1FB6"), true),
                new VehicleDTO("VLUR4X20009093588","DEF456",Guid.Parse("EFB499FA-B179-4B99-9539-6925751F1FB6"), false),
                new VehicleDTO("VLUR4X20009048066","GHI789",Guid.Parse("EFB499FA-B179-4B99-9539-6925751F1FB6"), true)
            });
            var customer1 = new CustomerDTO(Guid.Parse("EFB499FA-B179-4B99-9539-6925751F1FB6"), "Kalles Grustransporter AB", "Cementvägen 8, 111 11 Södertälje", customer1Vehicles);

            var customer2Vehicles = new List<VehicleDTO>();
            customer2Vehicles.AddRange(new List<VehicleDTO>() {
                new VehicleDTO("YS2R4X20005388011","JKL012",Guid.Parse("A0860071-B1B8-4663-AAD6-6D75A6C92D47"), true),
                new VehicleDTO("YS2R4X20005387949","MNO345",Guid.Parse("A0860071-B1B8-4663-AAD6-6D75A6C92D47"), false)
            });
            var customer2 = new CustomerDTO(Guid.Parse("A0860071-B1B8-4663-AAD6-6D75A6C92D47"), "Johans Bulk AB", "Balkvägen 12, 222 22 Stockholm", customer2Vehicles);

            var customer3Vehicles = new List<VehicleDTO>();
            customer3Vehicles.AddRange(new List<VehicleDTO>() {
                new VehicleDTO("YS2R4X20005387765","ABC123",Guid.Parse("D47CEC83-BCE8-46ED-B77D-D33D457319F7"), true),
                new VehicleDTO("YS2R4X20005387055","DEF456",Guid.Parse("D47CEC83-BCE8-46ED-B77D-D33D457319F7"), false)
            });
            var customer3 = new CustomerDTO(Guid.Parse("D47CEC83-BCE8-46ED-B77D-D33D457319F7"), "Haralds Värdetransporter AB", "Budgetvägen 1, 333 33 Uppsala", customer3Vehicles);

            _customers.Add(customer1);
            _customers.Add(customer2);
            _customers.Add(customer3);
        }
        private static void FillCustomersWithConnectedVehiclesSample()
        {
            _customersWithConnectedVehicles = new List<CustomerDTO>();
            var customer1Vehicles = new List<VehicleDTO>();
            customer1Vehicles.AddRange(new List<VehicleDTO>() {
                new VehicleDTO("YS2R4X20005399401","ABC123",Guid.Parse("EFB499FA-B179-4B99-9539-6925751F1FB6"), true),
                new VehicleDTO("VLUR4X20009048066","GHI789",Guid.Parse("EFB499FA-B179-4B99-9539-6925751F1FB6"), true)
            });
            var customer1 = new CustomerDTO(Guid.Parse("EFB499FA-B179-4B99-9539-6925751F1FB6"), "Kalles Grustransporter AB", "Cementvägen 8, 111 11 Södertälje", customer1Vehicles);

            var customer2Vehicles = new List<VehicleDTO>();
            customer2Vehicles.AddRange(new List<VehicleDTO>() {
                new VehicleDTO("YS2R4X20005388011","JKL012",Guid.Parse("A0860071-B1B8-4663-AAD6-6D75A6C92D47"), true)
            });
            var customer2 = new CustomerDTO(Guid.Parse("A0860071-B1B8-4663-AAD6-6D75A6C92D47"), "Johans Bulk AB", "Balkvägen 12, 222 22 Stockholm", customer2Vehicles);

            var customer3Vehicles = new List<VehicleDTO>();
            customer3Vehicles.AddRange(new List<VehicleDTO>() {
                new VehicleDTO("YS2R4X20005387765","ABC123",Guid.Parse("D47CEC83-BCE8-46ED-B77D-D33D457319F7"), true)
            });
            var customer3 = new CustomerDTO(Guid.Parse("D47CEC83-BCE8-46ED-B77D-D33D457319F7"), "Haralds Värdetransporter AB", "Budgetvägen 1, 333 33 Uppsala", customer3Vehicles);

            _customersWithConnectedVehicles.Add(customer1);
            _customersWithConnectedVehicles.Add(customer2);
            _customersWithConnectedVehicles.Add(customer3);
        }
        #endregion
    }
}
