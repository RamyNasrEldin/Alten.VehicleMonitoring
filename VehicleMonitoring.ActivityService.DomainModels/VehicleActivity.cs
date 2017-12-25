using System;
using System.ComponentModel.DataAnnotations;

namespace VehicleMonitoring.ActivityService.DomainModels
{
    public partial class VehicleActivity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(25)]
        public string VehicleId { get; set; }
        public bool Status { get; set; }
        public DateTime EntryDate { get; set; }
        
    }
}
