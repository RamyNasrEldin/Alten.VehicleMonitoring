using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleMonitoring.VehicleService.DomainModels
{
    public abstract class BaseModel
    {
        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}
