using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceMissionsTracker.Core.DTOs
{
    public class AddAgencyRequestDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public int Founded { get; set; }
    }
}
