using System;

namespace Inspiration_International.Helpers
{
    public static class ExtensionMethods
    {
        public static DateTime Next(this DateTime from, DayOfWeek dayOfWeek)
        {
            if (DateTime.Now.DayOfWeek == dayOfWeek)
            { return DateTime.Now; }
            int start = (int)from.DayOfWeek;
            int target = (int)dayOfWeek;
            if (target <= start)
                target += 7;
            return from.AddDays(target - start);
        }
    }
}