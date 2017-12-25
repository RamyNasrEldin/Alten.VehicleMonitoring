using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VehicleMonitoring.VehicleService.DomainModels
{
    public partial class Vehicle: BaseModel
    {
        [Required]
        [StringLength(25)]
        [Key]
        public string Id { get; set; }

        [StringLength(10)]
        public string RegNo { get; set; }

        public Guid? CustomerId { get; set; }
        
        public bool? CurrentStatus { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
