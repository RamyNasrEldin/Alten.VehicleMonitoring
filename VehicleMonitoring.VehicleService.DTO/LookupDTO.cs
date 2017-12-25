using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleMonitoring.VehicleService.DTO
{
    public class LookupDTO
    {
        #region properties
        public Guid ID { get; set; }
        public string Name { get; set; }
        #endregion

        #region CTORS
        public LookupDTO(Guid id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
        #endregion

    }
}
