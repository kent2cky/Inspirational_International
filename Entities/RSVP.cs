using System;

namespace Inspiration_International.Entities
{
    public class RSVP
    {
        public int RsvpID { get; set; }
        public string UserID { get; set; }
        public DateTime DateFor { get; set; } // The date of the next class which he is signing up to attend.
        public bool DidAttend { get; set; } // Did she/he actually attend?
    }
}