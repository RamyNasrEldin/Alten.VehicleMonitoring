using Microsoft.AspNetCore.Mvc;
using VehicleMonitoring.VehicleService.Infrastructure.UnitOfWork;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VehicleMonitoring.Core.FilterAttributes;

namespace VehicleMonitoring.VehicleService.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Customers")]
    public class CustomersController : Controller
    {
        #region Data Members
        private IVehicleServiceUOW _uow;
        private ILogger<CustomersController> _logger;
        public GeneralAppSettings _config;
        #endregion
        #region CTOR
        public CustomersController(IVehicleServiceUOW uow, ILogger<CustomersController> logger, IOptions<GeneralAppSettings> configOptions)
        {
            _uow = uow;
            _logger = logger;
            _config = configOptions.Value;
        }
        #endregion
        #region Actions
        /// <summary>
        /// Get list of cutomers as JSON
        /// </summary>
        /// <returns></returns>
        public JsonResult Get()
        {
            try
            {
                return Json(_uow.GetCustomersLookup());
            }
            catch(Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw new InvalidOperationException("Ops! we can't process your request currently. Please try again later.");
            }
        }
        #endregion
    }
}
