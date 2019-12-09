using System;
using System.ComponentModel.DataAnnotations;

namespace Inspiration_International.Models
{
    public class PhoneNumberModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a valid phone number.")]
        [StringLength(maximumLength: 15, MinimumLength = 11, ErrorMessage = "Length must be 11 digits minimum and 15 digits maximum.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}