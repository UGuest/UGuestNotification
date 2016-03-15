namespace ILuffy.UGuest.Repository
{
    using System;

    static class DateTimeHelper
    {
        public static string ToDefaultString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH-mm-ss");
        }

        public static DateTime ToDataTimeWithDefaultStyle(this string dateTimeAsString)
        {
            if(!string.IsNullOrEmpty(dateTimeAsString))
            {
                return DateTime.ParseExact(dateTimeAsString, "yyyy-MM-dd HH-mm-ss", null);
            }

            return DateTime.MinValue;
        }
    }
}
