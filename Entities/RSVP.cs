using System;

namespace Inspiration_International.Entities
{
    public class RSVP
    {
        public int RsvpID { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; } // Email or phone number of the personName
        public DateTime DateFor { get; set; } // The date of the next class which he is signing up to attend.
        public bool DidAttend { get; set; } // Did she/he actually attend?
    }
}