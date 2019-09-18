using System;

namespace Inspiration_International.Helpers
{
    public class Helper
    {
        public bool AllIsValid(DateTime dp, string title, string articleBody, string author)
        {
            if ((articleBody != null && author != null) && (dp != null && title != null))
                return true;
            return false;
        }

        public bool DateIsValid(DateTime date)
        {
            if (DateTime.Now.Date.Equals(date.Date))
                return true;
            return false;
        }
    }
}