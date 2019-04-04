using System;

namespace Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToEasternStandardTime(this DateTime dateTime)
        {
            var utcTime = TimeZoneInfo.ConvertTimeToUtc(dateTime);
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
        }
    }
}