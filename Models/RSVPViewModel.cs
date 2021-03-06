using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inspiration_International.Models
{
    public class RSVPViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a valid phone number.")]
        [StringLength(maximumLength: 15, MinimumLength = 11, ErrorMessage = "Length must be 11 digits minimum.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public bool RSVP { get; set; }
        public string FirstName { get; set; }

    }

}