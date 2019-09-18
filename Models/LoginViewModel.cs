using System;
using System.ComponentModel.DataAnnotations;

namespace Inspiration_International.Models
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter email or username.")]
        [StringLength(maximumLength: 25, MinimumLength = 3, ErrorMessage = "Length must be between 3 to 35")]
        public string Login { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}