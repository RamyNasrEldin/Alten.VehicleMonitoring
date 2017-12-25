using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleMonitoring.Core.Repository;
using VehicleMonitoring.ActivityService.Data;
using VehicleMonitoring.ActivityService.DomainModels;
using VehicleMonitoring.ActivityService.DTO;
using VehicleMonitoring.ActivityService.Infrastructure.UnitOfWork;

namespace VehicleMonitoring.VehicleService.Infrastructure.Tests
{
    [TestClass]
    public class VehicleActivityServiceUowTests
    {
        #region Data Members
        private static List<LookupDTO> _customersLookups;
        private static List<VehicleActivity> _vehiclesActivities;

        private static ILogger<VehicleActivityServiceUOW> _mockLogger;
        private static ActivityServiceDbContext _mockServiceDbContext;
        private static Mock<IRepositoryProvider> _mockRepositoryProvider;
        private static Mock<IRepository<VehicleActivity>> _mockVehicleActivitiesRepo;

        private static VehicleActivityServiceUOW _vehicleServiceUow;
        #endregion
        #region Test Attributes
        [ClassInitialize]
        public static void BeforeAllTests(TestContext context)
        {
            _mockLogger = Mock.Of<ILogger<VehicleActivityServiceUOW>>();
            var optionsBuilder = new DbContextOptionsBuilder<ActivityServiceDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            _mockServiceDbContext = new ActivityServiceDbContext(optionsBuilder.Options);

            _mockRepositoryProvider = new Mock<IRepositoryProvider>();
            _mockVehicleActivitiesRepo = new Mock<IRepository<VehicleActivity>>();

            FillVehiclesActivitiesListSample();

            _mockRepositoryProvider.Setup(rep => rep.DbContext).Returns(_mockServiceDbContext);
        }
        [TestInitialize]
        public void BeforeEachTest()
        {
            _mockVehicleActivitiesRepo = new Mock<IRepository<VehicleActivity>>();
        }
        #endregion
        #region Test Methods

        [TestMethod]
        public void SaveVehicleActivityTransaction_ShouldAddNewActivity()
        {
            // Arrange
            var testVehicleActivity = new VehicleActivityDTO() { VehicleId = "YS2R4X20005399401", Status = false, EntryDate = DateTime.Now };
            _mockVehicleActivitiesRepo.Setup(rep => rep.Add(testVehicleActivity.GetDALObj())).Returns(testVehicleActivity.GetDALObj());
            _mockVehicleActivitiesRepo.Setup(rep => rep.SaveChangesAsync()).Returns(() => Task<int>.Run(() => { return 1; }));
            _mockRepositoryProvider.Setup(rep => rep.GetRepositoryForEntityType<VehicleActivity>()).Returns(_mockVehicleActivitiesRepo.Object);

            _vehicleServiceUow = new VehicleActivityServiceUOW(_mockRepositoryProvider.Object, _mockLogger);

            // Act
            try
            {
                _vehicleServiceUow.SaveVehicleActivityTransaction(testVehicleActivity);
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected no exception, but got: " + ex.Message);
            }


        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void SaveVehicleActivityTransaction_ReturnsException_WithInternalExceptionOccures()
        {
            //Arrange
            _mockVehicleActivitiesRepo.Setup(rep => rep.Add(It.IsAny<VehicleActivity>())).Throws<Exception>();
            _mockRepositoryProvider.Setup(rep => rep.GetRepositoryForEntityType<VehicleActivity>()).Returns(_mockVehicleActivitiesRepo.Object);
            _vehicleServiceUow = new VehicleActivityServiceUOW(_mockRepositoryProvider.Object, _mockLogger);

            // Act
            _vehicleServiceUow.SaveVehicleActivityTransaction(new VehicleActivityDTO());
        }

        #endregion
        #region Helper Methods
        private static void FillVehiclesActivitiesListSample()
        {
            _vehiclesActivities = new List<VehicleActivity>();
            _vehiclesActivities.AddRange(new List<VehicleActivity>() {
                new VehicleActivity { VehicleId = "YS2R4X20005399401",Status=true,EntryDate=DateTime.Now },
                new VehicleActivity { VehicleId= "VLUR4X20009093588", Status=false, EntryDate=DateTime.Now },
                new VehicleActivity { VehicleId = "VLUR4X20009048066", Status=false, EntryDate=DateTime.Now }
            });
        }
        #endregion
    }
}