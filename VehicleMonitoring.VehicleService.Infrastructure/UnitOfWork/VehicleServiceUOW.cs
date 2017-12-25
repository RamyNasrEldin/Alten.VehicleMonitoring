using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using VehicleMonitoring.Core.Repository;
using VehicleMonitoring.VehicleService.DTO;
using VehicleMonitoring.VehicleService.DomainModels;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VehicleMonitoring.VehicleService.Infrastructure.UnitOfWork
{
    public class VehicleServiceUOW : IVehicleServiceUOW, IDisposable
    {
        #region Data Members 
        /// <summary>
        /// repositories
        /// </summary>
        protected IRepositoryProvider RepositoryProvider { get; set; }
        protected IRepository<Customer> CustomersRepo { get { return GetStandardRepo<Customer>(); } }
        protected IRepository<Vehicle> VehiclesRepo { get { return GetStandardRepo<Vehicle>(); } }

        private ILogger<VehicleServiceUOW> _logger;
        #endregion
        #region Constructor
        public VehicleServiceUOW(IRepositoryProvider repositoryProvider, ILogger<VehicleServiceUOW> logger)
        {
            if (repositoryProvider.DbContext == null) throw new ArgumentNullException("dbContext is null"); /// if Database context not initalized Through Exception
            this.RepositoryProvider = repositoryProvider;
            _logger = logger;
        }
        #endregion
        #region Public Methods
        public List<LookupDTO> GetCustomersLookup()
        {
            try
            {
                List<LookupDTO> customersLookups = new List<LookupDTO>();
                var customers = CustomersRepo.All().Select(c => new { c.Id, c.Name }).ToList();
                foreach (var customer in customers)
                {
                    customersLookups.Add(new LookupDTO(customer.Id, customer.Name));
                }
                return customersLookups;
            }
            catch(Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw ex;
            }
            
        }
        public List<CustomerDTO> GetCustomersVehicles()
        {
            try
            { 
            var customersList = CustomersRepo.All().Include(v => v.Vehicles).ToList();
            return CustomerDTO.GetList(customersList).ToList();

            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw ex;
            }
        }
        public List<CustomerDTO> GetCustomersVehiclesByStatus(bool status)
        {
            var customersList = CustomersRepo.All().Include(c => c.Vehicles.Where(v=>v.CurrentStatus == status)).ToList();
            return CustomerDTO.GetList(customersList).ToList();
        }
        public CustomerDTO GetCustomerVehiclesByCustomerId(Guid customerID)
        {
            try
            {
                var customers = CustomersRepo.All().Where(c => c.Id == customerID).Include(v => v.Vehicles);
                if (customers.Count() > 0)
                {
                    return new CustomerDTO(customers.FirstOrDefault());
                }

                return null;
            }
            catch(Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw ex;
            }
        }
        public async Task<bool> UpdateVehicleStatus(string VehicleId, bool Status)
        {
            try
            {
                var vehcile = VehiclesRepo.FindById(VehicleId);
                if (vehcile != null)
                {
                    vehcile.CurrentStatus = Status;
                    vehcile.LastUpdateTime = DateTime.Now;
                    VehiclesRepo.Update(vehcile);
                    await VehiclesRepo.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw ex;
            }
        }
        #endregion
        #region Private Methods
        private IRepository<T> GetStandardRepo<T>() where T : class
        {
            try
            {
                var repo = RepositoryProvider.GetRepositoryForEntityType<T>();
                return repo;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw ex;
            }
        }
        private T GetRepo<T>() where T : class
        {
            try
            {
                return RepositoryProvider.GetRepository<T>();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw ex;
            }
        }
        #endregion
        #region Disposing 
       
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
