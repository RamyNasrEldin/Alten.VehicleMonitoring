using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleMonitoring.Core.Repository;
using VehicleMonitoring.VehicleService.Data;
using VehicleMonitoring.VehicleService.DomainModels;
using VehicleMonitoring.VehicleService.DTO;
using VehicleMonitoring.VehicleService.Infrastructure.UnitOfWork;

namespace VehicleMonitoring.VehicleService.Infrastructure.Tests
{
    [TestClass]
    public class VehicleServiceUowTests
    {
        #region Data Members
        private static List<LookupDTO> _customersLookups;
        private static List<Customer> _customers;
        private static List<Vehicle> _vehicles;
        private static List<Customer> _customersWithConnectedVehicles;

        private static ILogger<VehicleServiceUOW> _mockLogger;
        private static VehicleServiceDbContext _mockServiceDbContext;
        private static Mock<IRepositoryProvider> _mockRepositoryProvider;
        private static Mock<IRepository<Customer>> _mockCustomersRepo;
        private static Mock<IRepository<Vehicle>> _mockVehiclesRepo;

        private static VehicleServiceUOW _vehicleServiceUow;
        #endregion
        #region Test Attributes
        [ClassInitialize]
        public static void BeforeAllTests(TestContext context)
        {
            _mockLogger = Mock.Of<ILogger<VehicleServiceUOW>>();
            var optionsBuilder = new DbContextOptionsBuilder<VehicleServiceDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            _mockServiceDbContext = new VehicleServiceDbContext(optionsBuilder.Options);

            _mockRepositoryProvider = new Mock<IRepositoryProvider>();
            _mockCustomersRepo = new Mock<IRepository<Customer>>();
            _mockVehiclesRepo = new Mock<IRepository<Vehicle>>();

            FillCustomersLookupsDTOsListSample();
            FillCustomersWithConnectedVehiclesSample();
            FillCustomersListSample();
            FillVehiclesListSample();

            _mockRepositoryProvider.Setup(rep => rep.DbContext).Returns(_mockServiceDbContext);
        }
        [TestInitialize]
        public void BeforeEachTest()
        {
            _mockCustomersRepo = new Mock<IRepository<Customer>>();
            _mockVehiclesRepo = new Mock<IRepository<Vehicle>>();
        }
        #endregion
        #region Test Methods
        [TestMethod]
        public void GetCustomersLookup_ReturnsListOfLookupDTOs_WithAnyRequest()
        {
            // Arrange
            _mockCustomersRepo.Setup(rep => rep.All()).Returns(_customers.AsQueryable());
            _mockRepositoryProvider.Setup(rep => rep.GetRepositoryForEntityType<Customer>()).Returns(_mockCustomersRepo.Object);
            _vehicleServiceUow = new VehicleServiceUOW(_mockRepositoryProvider.Object, _mockLogger);

            // Act
            var result = _vehicleServiceUow.GetCustomersLookup();

            // Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<LookupDTO>));
            Assert.AreEqual(3, result.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetCustomersLookup_ReturnsException_WithInternalExceptionOccures()
        {
            //Arrange
            _mockCustomersRepo.Setup(rep => rep.All()).Throws<Exception>();
            _mockRepositoryProvider.Setup(rep => rep.GetRepositoryForEntityType<Customer>()).Returns(_mockCustomersRepo.Object);
            _vehicleServiceUow = new VehicleServiceUOW(_mockRepositoryProvider.Object, _mockLogger);

            // Act
            _vehicleServiceUow.GetCustomersLookup();
        }

        [TestMethod]
        public void GetCustomersVehicles_ReturnsListOfCustomersDTOs_WithAnyRequest()
        {
            // Arrange
            _mockCustomersRepo.Setup(rep => rep.All()).Returns(_customers.AsQueryable());
            _mockRepositoryProvider.Setup(rep => rep.GetRepositoryForEntityType<Customer>()).Returns(_mockCustomersRepo.Object);
            _vehicleServiceUow = new VehicleServiceUOW(_mockRepositoryProvider.Object, _mockLogger);

            // Act
            var result = _vehicleServiceUow.GetCustomersVehicles();

            // Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<CustomerDTO>));
            Assert.AreEqual(3, result.Count);
        }
        [TestMethod]
        public void GetCustomersVehicles_ReturnsListOfCustomersDTOs_WithVehiclesListDTOsForEachCustomer()
        {
            // Arrange
            _mockCustomersRepo.Setup(rep => rep.All()).Returns(_customers.AsQueryable());
            _mockRepositoryProvider.Setup(rep => rep.GetRepositoryForEntityType<Customer>()).Returns(_mockCustomersRepo.Object);
            _vehicleServiceUow = new VehicleServiceUOW(_mockRepositoryProvider.Object, _mockLogger);

            // Act
            var result = _vehicleServiceUow.GetCustomersVehicles();

            // Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<CustomerDTO>));
            Assert.AreEqual(3, result.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetCustomersVehicles_ReturnsException_WithInternalExceptionOccures()
        {
            //Arrange
            _mockCustomersRepo.Setup(rep => rep.All()).Throws<Exception>();
            _mockRepositoryProvider.Setup(rep => rep.GetRepositoryForEntityType<Customer>()).Returns(_mockCustomersRepo.Object);
            _vehicleServiceUow = new VehicleServiceUOW(_mockRepositoryProvider.Object, _mockLogger);

            // Act
            _vehicleServiceUow.GetCustomersVehicles();
        }

        [TestMethod]
        public void GetCustomersVehiclesByStatus_ReturnsListOfCustomersDTOs_WithSpecificStatus()
        {
            // Arrange
            _mockCustomersRepo.Setup(rep => rep.All()).Returns(_customersWithConnectedVehicles.AsQueryable());
            _mockRepositoryProvider.Setup(rep => rep.GetRepositoryForEntityType<Customer>()).Returns(_mockCustomersRepo.Object);
            _vehicleServiceUow = new VehicleServiceUOW(_mockRepositoryProvider.Object, _mockLogger);

            // Act
            var result = _vehicleServiceUow.GetCustomersVehiclesByStatus(true);

            // Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<CustomerDTO>));
            Assert.AreEqual(3, result.Count);
        }
        [TestMethod]
        public void GetCustomersVehiclesByStatus_ReturnsListOfCustomersDTOs_WithVehiclesListDTOsForEachCustomer_EachVehicleHasTheSameSentStatus()
        {
            // Arrange
            _mockCustomersRepo.Setup(rep => rep.All()).Returns(_customersWithConnectedVehicles.AsQueryable());
            _mockRepositoryProvider.Setup(rep => rep.GetRepositoryForEntityType<Customer>()).Returns(_mockCustomersRepo.Object);
            _vehicleServiceUow = new VehicleServiceUOW(_mockRepositoryProvider.Object, _mockLogger);

            // Act
            var result = _vehicleServiceUow.GetCustomersVehiclesByStatus(true);

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(2, result[0].Vehicles.Where(v => v.CurrentStatus).Count());
            Assert.AreEqual(1, result[1].Vehicles.Where(v => v.CurrentStatus).Count());
            Assert.AreEqual(1, result[2].Vehicles.Where(v => v.CurrentStatus).Count());
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetCustomersVehiclesByStatus_ReturnsException_WithInternalExceptionOccures()
        {
            //Arrange
            _mockCustomersRepo.Setup(rep => rep.All()).Throws<Exception>();
            _mockRepositoryProvider.Setup(rep => rep.GetRepositoryForEntityType<Customer>()).Returns(_mockCustomersRepo.Object);
            _vehicleServiceUow = new VehicleServiceUOW(_mockRepositoryProvider.Object, _mockLogger);

            // Act
            _vehicleServiceUow.GetCustomersVehiclesByStatus(true);
        }

        [TestMethod]
        public void GetCustomerVehiclesByCustomerId_ReturnsCustomerDTO_WithCustomerId()
        {
            // Arrange
            var testCustomer = new CustomerDTO(_customers.SingleOrDefault(c => c.Id == new Guid("EFB499FA-B179-4B99-9539-6925751F1FB6")));
            _mockCustomersRepo.Setup(rep => rep.All()).Returns(_customers.AsQueryable());
            _mockRepositoryProvider.Setup(rep => rep.GetRepositoryForEntityType<Customer>()).Returns(_mockCustomersRepo.Object);
            _vehicleServiceUow = new VehicleServiceUOW(_mockRepositoryProvider.Object, _mockLogger);

            // Act
            var result = _vehicleServiceUow.GetCustomerVehiclesByCustomerId(new Guid("EFB499FA-B179-4B99-9539-6925751F1FB6"));

            // Assert
            Assert.IsInstanceOfType(result, typeof(CustomerDTO));
            Assert.AreEqual(testCustomer.CustomerId, result.CustomerId);
            Assert.AreEqual(testCustomer.Name, result.Name);
            Assert.AreEqual(testCustomer.Address, result.Address);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetCustomerVehiclesByCustomerId_ReturnsException_WithInternalExceptionOccures()
        {
            //Arrange
            _mockCustomersRepo.Setup(rep => rep.All()).Throws<Exception>();
            _mockRepositoryProvider.Setup(rep => rep.GetRepositoryForEntityType<Customer>()).Returns(_mockCustomersRepo.Object);
            _vehicleServiceUow = new VehicleServiceUOW(_mockRepositoryProvider.Object, _mockLogger);

            // Act
            _vehicleServiceUow.GetCustomersVehiclesByStatus(true);
        }


        [TestMethod]
        public void UpdateVehicleStatus_ReturnsTrueResult()
        {
            // Arrange
            var testVehicle = _vehicles.FirstOrDefault(v => v.Id == "YS2R4X20005399401");
            _mockVehiclesRepo.Setup(rep => rep.FindById("YS2R4X20005399401")).Returns(testVehicle);
            _mockVehiclesRepo.Setup(rep => rep.SaveChangesAsync()).Returns(() => Task<int>.Run(() => { return 1; }));
            _mockRepositoryProvider.Setup(rep => rep.GetRepositoryForEntityType<Vehicle>()).Returns(_mockVehiclesRepo.Object);

            _vehicleServiceUow = new VehicleServiceUOW(_mockRepositoryProvider.Object, _mockLogger);

            // Act
            var result = _vehicleServiceUow.UpdateVehicleStatus("YS2R4X20005399401", false);

            // Assert
            Assert.AreEqual(true, result.Result);
        }
        [TestMethod]
        public void UpdateVehicleStatus_ShouldUpdateTargetVehicleStatus_WithSentStatus()
        {
            // Arrange
            var testVehicle = _vehicles.FirstOrDefault(v => v.Id == "YS2R4X20005399401");
            _mockVehiclesRepo.Setup(rep => rep.FindById("YS2R4X20005399401")).Returns(testVehicle);
            _mockVehiclesRepo.Setup(rep => rep.SaveChangesAsync()).Returns(() => Task<int>.Run(() => { return 1; }));
            _mockRepositoryProvider.Setup(rep => rep.GetRepositoryForEntityType<Vehicle>()).Returns(_mockVehiclesRepo.Object);

            _vehicleServiceUow = new VehicleServiceUOW(_mockRepositoryProvider.Object, _mockLogger);

            // Act
            var result = _vehicleServiceUow.UpdateVehicleStatus("YS2R4X20005399401", false);

            // Assert
            Assert.AreEqual(false, testVehicle.CurrentStatus);
        }
        #endregion
        #region Helper Methods
        private static void FillCustomersLookupsDTOsListSample()
        {
            _customersLookups = new List<LookupDTO>()
            {
                new LookupDTO(Guid.Parse("EFB499FA-B179-4B99-9539-6925751F1FB6"), "Kalles Grustransporter AB"),
                new LookupDTO(Guid.Parse("A0860071-B1B8-4663-AAD6-6D75A6C92D47"), "Johans Bulk AB"),
                new LookupDTO(Guid.Parse("D47CEC83-BCE8-46ED-B77D-D33D457319F7"), "Haralds Värdetransporter AB")
            };
        }
        private static void FillCustomersListSample()
        {
            _customers = new List<Customer>();
            _customers.AddRange(new List<Customer>() {
               new Customer { Id = Guid.Parse("EFB499FA-B179-4B99-9539-6925751F1FB6"), Name = "Kalles Grustransporter AB", Address = "Cementvägen 8, 111 11 Södertälje", IsActive = true, CreatedOn = DateTime.Now, IsDeleted = false },
               new Customer { Id = Guid.Parse("A0860071-B1B8-4663-AAD6-6D75A6C92D47"), Name = "Johans Bulk AB", Address = "Balkvägen 12, 222 22 Stockholm", IsActive = true, CreatedOn = DateTime.Now, IsDeleted = false },
               new Customer { Id = Guid.Parse("D47CEC83-BCE8-46ED-B77D-D33D457319F7"), Name = "Haralds Värdetransporter AB", Address = "Budgetvägen 1, 333 33 Uppsala", IsActive = true, CreatedOn = DateTime.Now, IsDeleted = false }
               });
        }
        private static void FillVehiclesListSample()
        {
            _vehicles = new List<Vehicle>();
            _vehicles.AddRange(new List<Vehicle>() {
                new Vehicle { Id = "YS2R4X20005399401", RegNo = "ABC123", CustomerId = Guid.Parse("EFB499FA-B179-4B99-9539-6925751F1FB6"), CurrentStatus = true, IsActive = true, IsDeleted = false, LastUpdateTime = DateTime.Now, CreatedOn = DateTime.Now },
                new Vehicle { Id = "VLUR4X20009093588", RegNo = "DEF456", CustomerId = Guid.Parse("EFB499FA-B179-4B99-9539-6925751F1FB6"), CurrentStatus = false, IsActive = true, IsDeleted = false, LastUpdateTime = DateTime.Now, CreatedOn = DateTime.Now },
                new Vehicle { Id = "VLUR4X20009048066", RegNo = "GHI789", CustomerId = Guid.Parse("EFB499FA-B179-4B99-9539-6925751F1FB6"), CurrentStatus = true, IsActive = true, IsDeleted = false, LastUpdateTime = DateTime.Now, CreatedOn = DateTime.Now }
            });
        }

        private static void FillCustomersWithConnectedVehiclesSample()
        {
            _customersWithConnectedVehicles = new List<Customer>();
            var customer1Vehicles = new List<Vehicle>();
            customer1Vehicles.AddRange(new List<Vehicle>() {
                new Vehicle{ Id= "YS2R4X20005399401",RegNo="ABC123",CustomerId= Guid.Parse("EFB499FA-B179-4B99-9539-6925751F1FB6"),CurrentStatus= true },
                new Vehicle{Id="VLUR4X20009048066",RegNo="GHI789",CustomerId= Guid.Parse("EFB499FA-B179-4B99-9539-6925751F1FB6"),CurrentStatus= true }
            });
            var customer1 = new Customer { Id = Guid.Parse("EFB499FA-B179-4B99-9539-6925751F1FB6"), Name = "Kalles Grustransporter AB", Address = "Cementvägen 8, 111 11 Södertälje", Vehicles = customer1Vehicles };

            var customer2Vehicles = new List<Vehicle>();
            customer2Vehicles.AddRange(new List<Vehicle>() {
                new Vehicle{Id="YS2R4X20005388011",RegNo="JKL012",CustomerId= Guid.Parse("A0860071-B1B8-4663-AAD6-6D75A6C92D47"),CurrentStatus= true }
            });
            var customer2 = new Customer { Id = Guid.Parse("A0860071-B1B8-4663-AAD6-6D75A6C92D47"), Name = "Johans Bulk AB", Address = "Balkvägen 12, 222 22 Stockholm", Vehicles = customer2Vehicles };

            var customer3Vehicles = new List<Vehicle>();
            customer3Vehicles.AddRange(new List<Vehicle>() {
                new Vehicle{Id= "YS2R4X20005387765",RegNo="ABC123",CustomerId= Guid.Parse("D47CEC83-BCE8-46ED-B77D-D33D457319F7"), CurrentStatus= true }
            });
            var customer3 = new Customer { Id = Guid.Parse("D47CEC83-BCE8-46ED-B77D-D33D457319F7"), Name = "Haralds Värdetransporter AB", Address = "Budgetvägen 1, 333 33 Uppsala", Vehicles = customer3Vehicles };

            _customersWithConnectedVehicles.Add(customer1);
            _customersWithConnectedVehicles.Add(customer2);
            _customersWithConnectedVehicles.Add(customer3);
        }
        #endregion
    }
}