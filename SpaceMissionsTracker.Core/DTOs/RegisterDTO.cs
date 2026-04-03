using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceMissionsTracker.Core.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Person Name can't be blank")]
        public string PersonName { get; set; }
        [Required(ErrorMessage = "Email Name can't be blank")]
        [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone Name can't be blank")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Password Name can't be blank")]
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
