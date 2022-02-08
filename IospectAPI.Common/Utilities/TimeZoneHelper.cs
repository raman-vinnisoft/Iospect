using System;

namespace IospectAPI.Common.Utilities
{
    public static class TimeZoneHelper
    {
        public static DateTime ConvertTimeToUsersTimeZone(DateTime utcDate, string timeZoneId)
        {
            TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            utcDate = DateTime.SpecifyKind(utcDate, DateTimeKind.Utc);
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcDate, localTimeZone);
            return localTime;
        }

        public static TimeSpan? ConvertTimeToUsersTimeZone(TimeSpan? timespan, string timeZoneId)
        {
            if (timespan == null)
            {
                return null;
            }
            DateTime dateTime = new DateTime(DateTime.Now.Year,
                                                   DateTime.Now.Month, DateTime.Now.Day, timespan.Value.Hours,
                                                   timespan.Value.Minutes, timespan.Value.Seconds);
            TimeZoneInfo timezone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            DateTime dateTimeInTimeZone = TimeZoneInfo.ConvertTimeFromUtc(dateTime, timezone);
            return dateTimeInTimeZone.TimeOfDay;
        }
    }
}
