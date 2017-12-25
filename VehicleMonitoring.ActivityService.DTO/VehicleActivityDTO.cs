using System;
using System.Collections.Generic;
using System.Text;
using VehicleMonitoring.ActivityService.DomainModels;

namespace VehicleMonitoring.ActivityService.DTO
{
    /// <summary>
    /// Data transfer Objects, used for simplifying DAL obejcts to the client to deliver only the data scope needed for the client 
    /// Exposing DAL objects directly to is not recommended 
    /// </summary>
    [Serializable]
    public class VehicleActivityDTO
    {
        #region Properties 

        public int ID { get; set; }
        public string VehicleId { get; set; }
        public DateTime EntryDate { get; set; }
        public bool Status { get; set; }
        #endregion

        #region CTOR
        public VehicleActivityDTO()
        {
        }
        public VehicleActivityDTO(string vehicleId, bool status)
        {
            this.VehicleId = vehicleId;
            this.Status = status;
            this.EntryDate = DateTime.Now;
        }
        public VehicleActivityDTO(VehicleActivity TransDAL)
        {
            this.ID = TransDAL.Id;
            this.VehicleId = TransDAL.VehicleId;
            this.EntryDate = TransDAL.EntryDate;
            this.Status = TransDAL.Status;
        }
        #endregion


        #region Map Helpers 
        /// <summary>
        /// Map Collection of DAL objects Into List of DTOs
        /// </summary>
        /// <param name="Collection"></param>
        /// <returns></returns>
        public static IEnumerable<VehicleActivityDTO> GetList(ICollection<VehicleActivity> Collection)
        {
            List<VehicleActivityDTO> list = new List<VehicleActivityDTO>();
            foreach (VehicleActivity Trans in Collection)
            {
                list.Add(new VehicleActivityDTO(Trans));
            }
            return list;
        }
        /// <summary>
        /// Map DTO to DAL Object
        /// </summary>
        /// <returns></returns>
        public VehicleActivity GetDALObj()
        {
            return new VehicleActivity()
            {
                VehicleId = this.VehicleId,
                EntryDate = this.EntryDate,
                Status = this.Status

            };

        }
        #endregion 
    }
}
