using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceMissionsTracker.Core.DTOs
{
    public class AddRocketRequestDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int AgencyId { get; set; }
        [Required]
        public int FirstLaunch { get; set; }
    }
}
