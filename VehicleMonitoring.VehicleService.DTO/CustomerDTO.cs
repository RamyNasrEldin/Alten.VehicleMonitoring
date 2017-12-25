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
    public class CustomerDTO
    {
        #region Data Members
        public System.Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public IEnumerable<VehicleDTO> Vehicles { get; set; }

        #endregion
        #region CTORS

        /// <summary>
        /// Full Constructor for mapping
        /// Avoid using Automapping for the sake of the performance 
        /// </summary>
        /// <param name="customerDAL"></param>
        public CustomerDTO(Customer customerDAL)
        {
            this.CustomerId = customerDAL.Id;
            this.Name = customerDAL.Name;
            this.Address = customerDAL.Address;
            this.Vehicles = new List<VehicleDTO>();
            if (customerDAL.Vehicles != null && customerDAL.Vehicles.Count > 0)
            {
                this.Vehicles = VehicleDTO.GetList(customerDAL.Vehicles);
            }
        }
        public CustomerDTO(Guid customerId, string name, string address, List<VehicleDTO> vehicles)
        {
            this.CustomerId = customerId;
            this.Name = name;
            this.Address = address;
            this.Vehicles = new List<VehicleDTO>();
            if (vehicles != null) { this.Vehicles = vehicles; }
        }
        #endregion
        #region Mapping helpers 
        /// <summary>
        /// Map Collection of DAL objects Into List of DTOs
        /// </summary>
        /// <param name="Collection"></param>
        /// <returns></returns>
        public static IEnumerable<CustomerDTO> GetList(ICollection<Customer> Collection)
        {
            List<CustomerDTO> list = new List<CustomerDTO>();
            foreach (Customer cus in Collection)
            {
                list.Add(new CustomerDTO(cus));
            }
            return list;
        }
        
        #endregion 


    }
}
