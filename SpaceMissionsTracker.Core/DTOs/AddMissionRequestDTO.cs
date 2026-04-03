using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceMissionsTracker.Core.DTOs
{
    public class AddMissionRequestDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int RocketId { get; set; }
        [Required]
        public DateTime LaunchDate { get; set; }
        [Required]
        public string Status { get; set; } = string.Empty;
        [Required]
        public string Destination { get; set; } = string.Empty;
    }
}
