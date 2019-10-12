using System;
using System.ComponentModel.DataAnnotations;

namespace Inspiration_International.Models
{
    public class FrontEndErrorReportModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a valid phone number.")]
        [StringLength(maximumLength: 30, MinimumLength = 5, ErrorMessage = "Length must be 5 characters at least")]
        public string ErrorMessage { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a valid phone number.")]
        [StringLength(maximumLength: 30, MinimumLength = 5, ErrorMessage = "Length must be 5 characters at least")]
        public string Sender { get; set; }
    }
}