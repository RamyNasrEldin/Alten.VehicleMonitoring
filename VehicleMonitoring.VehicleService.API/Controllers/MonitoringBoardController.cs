using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VehicleMonitoring.VehicleService.Infrastructure.UnitOfWork;

namespace VehicleMonitoring.VehicleService.API.Controllers
{
    [Produces("application/json")]
    [Route("api/MonitoringBoard")]
    public class MonitoringBoardController : Controller
    {

        #region Data Members
        private IVehicleServiceUOW _uow;
        private ILogger<MonitoringBoardController> _logger;
        public GeneralAppSettings _config;
        #endregion
        #region CTOR
        public MonitoringBoardController(IVehicleServiceUOW uow, ILogger<MonitoringBoardController> logger, IOptions<GeneralAppSettings> configOptions)
        {
            _uow = uow;
            _logger = logger;
            _config = configOptions.Value;
        }
        #endregion
        #region Actions
        [HttpGet("GetCustomersVehicles")]
        public JsonResult GetCustomersVehicles()
        {
            try
            {
                return Json(_uow.GetCustomersVehicles());
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw new InvalidOperationException("Ops! we can't process your request currently. Please try again later.");
            }
        }

        [HttpGet("GetCustomerVehiclesByCustomerId/{customerID}")]
        public JsonResult GetCustomerVehiclesByCustomerId(string customerID)
        {
            if (!string.IsNullOrEmpty(customerID))
            {
                try
                {
                    return Json(_uow.GetCustomerVehiclesByCustomerId(Guid.Parse(customerID)));
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex.Message);
                    throw new InvalidOperationException("Ops! we can't process your request currently. Please try again later.");
                }
            }
            else
            {
                _logger.LogCritical("Request without customer Id");
                throw new ArgumentException("Please provide valid customer Id.");
            }
        }

        [HttpGet("GetCustomersVehiclesByStatus/{status}")]
        public JsonResult GetCustomersVehiclesByStatus(bool status)
        {
            try
            {
                return Json(_uow.GetCustomersVehiclesByStatus(status));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw new InvalidOperationException("Ops! we can't process your request currently. Please try again later.");
            }
        }
        #endregion
    }
}