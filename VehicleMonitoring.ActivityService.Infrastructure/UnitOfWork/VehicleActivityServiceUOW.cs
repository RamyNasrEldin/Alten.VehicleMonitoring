using System;
using System.Collections.Generic;
using System.Text;
using VehicleMonitoring.Core.Repository;
using VehicleMonitoring.ActivityService.DTO;
using VehicleMonitoring.ActivityService.DomainModels;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VehicleMonitoring.ActivityService.Infrastructure.UnitOfWork
{
    public class VehicleActivityServiceUOW : IVehicleActivityServiceUOW
    {
        #region Data Members 
        /// <summary>
        /// repositories
        /// </summary>
        protected IRepositoryProvider RepositoryProvider { get; set; }
        protected IRepository<VehicleActivity> VehicleActivityRepo { get { return GetStandardRepo<VehicleActivity>(); } }
        private ILogger<VehicleActivityServiceUOW> _logger;
        private bool disposed = false;
        #endregion
        #region Constructor
        public VehicleActivityServiceUOW(IRepositoryProvider repositoryProvider, ILogger<VehicleActivityServiceUOW> logger)
        {
            if (repositoryProvider.DbContext == null) throw new ArgumentNullException("dbContext is null"); /// if Database context not initalized Through Exception
            this.RepositoryProvider = repositoryProvider;
            _logger = logger;
        }
        #endregion
        #region Public Methods
        public void SaveVehicleActivityTransaction(VehicleActivityDTO vehicleActivityDTO)
        {
            try
            {
                VehicleActivityRepo.Add(vehicleActivityDTO.GetDALObj());
                Commit();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw ex;
            }
        }

        public async Task<bool> SaveVehicleActivityTransactionAsync(VehicleActivityDTO vehicleActivityDTO)
        {
            try
            {
                VehicleActivityRepo.Add(vehicleActivityDTO.GetDALObj());
                await CommitAsync();
                return true;
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
                return RepositoryProvider.GetRepositoryForEntityType<T>();
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
        private async Task CommitAsync()
        {
            try
            {
                await VehicleActivityRepo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw ex;
            }
        }

        private void Commit()
        {
            try
            {
                this.RepositoryProvider.DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw ex;
            }
        }

        #endregion
        #region Disposing 
        protected virtual void Dispose(bool disposing)
        {

            if (!this.disposed)
            {
                this.Commit(); // Commiting the changes if any to the database before disposing and finalize the UoW object.

            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
