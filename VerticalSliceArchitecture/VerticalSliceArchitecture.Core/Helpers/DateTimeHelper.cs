using System;

namespace VerticalSliceArchitecture.Core.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime Now
            => NowImpl();

        public static Func<DateTime> NowImpl =
            () => DateTime.UtcNow;

        public static DateTime CreateNotificationDate(int timePeriodEnumValue, DateTime date)
        {
            switch (timePeriodEnumValue)
            {
                case 0:
                   break;
                case 1:
                    return date.AddDays(-1);
                case 2:
                    return date.AddDays(-2);
                case 3:
                    return date.AddDays(-3);
                case 4:
                    return date.AddDays(-4);
                case 5:
                    return date.AddDays(-5);
                case 6:
                    return date.AddDays(-6);
                case 7:
                    return date.AddDays(-7);
                case 8:
                    return date.AddDays(-14);
                case 9:
                    return date.AddDays(-21);
                case 10:
                    return date.AddMonths(-1);
            }
            return date;
        }
    }
}
