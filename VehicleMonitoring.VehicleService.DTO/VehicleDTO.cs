using System;
using System.Collections.Generic;
using System.Text;
using VehicleMonitoring.VehicleService.DomainModels;

namespace VehicleMonitoring.VehicleService.DTO
{
    /// <summary>
    /// Data transfer Objects, used for simplifying DAL obejcts to the client to deliver only the data scope needed for the client 
    /// Exposing DAL objects directly to is not recommended 
    /// </summary>

    [Serializable]
    public class VehicleDTO
    {
        #region Properties
        public string ID { get; set; }
        public string RegNo { get; set; }
        public Nullable<System.Guid> CustomerID { get; set; }
        public System.DateTime LastUpdateTime { get; set; }
        public bool CurrentStatus { get; set; }

        #endregion

        #region CTORS
        public VehicleDTO(Vehicle VehicleDAL)
        {
            this.ID = VehicleDAL.Id;
            this.RegNo = VehicleDAL.RegNo;
            this.CustomerID = VehicleDAL.CustomerId.Value;
            this.LastUpdateTime = VehicleDAL.LastUpdateTime.HasValue ? VehicleDAL.LastUpdateTime.Value : DateTime.Now;
            this.CurrentStatus = VehicleDAL.CurrentStatus.HasValue ? VehicleDAL.CurrentStatus.Value : false;
        }
        public VehicleDTO(string vehicleId, string regNo, Guid customerId, bool activeStatus, DateTime? lastUpdatedTime = null)
        {
            this.ID = vehicleId;
            this.RegNo = regNo;
            this.CustomerID = customerId;
            this.LastUpdateTime = lastUpdatedTime.HasValue ? lastUpdatedTime.Value : DateTime.Now;
            this.CurrentStatus = activeStatus;
        }
        #endregion

        #region Mapping helpers 
        /// <summary>
        /// Map Collection of DAL objects Into List of DTOs
        /// </summary>
        /// <param name="Collection"></param>
        /// <returns></returns>
        public static IEnumerable<VehicleDTO> GetList(ICollection<Vehicle> Collection)
        {
            List<VehicleDTO> list = new List<VehicleDTO>();
            foreach (Vehicle veh in Collection)
            {
                list.Add(new VehicleDTO(veh));
            }
            return list;
        }
        #endregion
    }
}
