// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeExtensions.cs" company="Skymate">
//      Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//   日期扩展
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Skymate.Extensions
{
    using System;
    using System.Globalization;
    using System.Xml;

    /// <summary>
    /// The date time extensions.
    /// 日期扩展
    /// </summary>
    public static class DateTimeExtensions
    {
        #region Fields

        /// <summary>
        /// The begin of epoch.
        /// </summary>
        public static readonly DateTime BeginOfEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// The min date.
        /// 最小日期
        /// </summary>
        private static readonly DateTime MinDate = new DateTime(1900, 1, 1);

        /// <summary>
        /// The max date.
        /// 最大日期
        /// </summary>
        private static readonly DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);

        #endregion Fields

        /// <summary>
        /// The is valid.
        /// 判断日期是否有效
        /// </summary>
        /// <param name="value">
        /// The value.
        /// 值
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>bool</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static bool IsValid(this DateTime value)
        {
            return (value >= MinDate) && (value <= MaxDate);
        }

        /// <summary>
        /// The get local offset.
        /// 获取本地日期便宜
        /// </summary>
        /// <param name="value">
        /// The value.
        /// 值
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>string</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static string GetLocalOffset(this DateTime value)
        {
            var utcOffset = TimeZoneInfo.Local.GetUtcOffset(value);
            return utcOffset.Hours.ToString("+00;-00", CultureInfo.InvariantCulture) + ":" + utcOffset.Minutes.ToString("00;00", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a nullable date/time value to UTC.
        /// 转换可空日期到UTC
        /// </summary>
        /// <param name="dateTime">
        /// The nullable date/time
        /// 可空日期
        /// </param>
        /// <returns>
        /// The nullable date/time in UTC
        /// </returns>
        public static DateTime? ToUniversalTime(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToUniversalTime() : (DateTime?)null;
        }

        /// <summary>
        /// Returns a copy of a date/time value with its kind
        /// set to <see cref="DateTimeKind.Utc"/> but does not perform
        /// any time-zone adjustment.
        /// </summary>
        /// <remarks>
        /// This method is useful when obtaining date/time values from sources
        /// that might not correctly set the UTC flag.
        /// </remarks>
        /// <param name="dateTime">
        /// The date/time
        /// </param>
        /// <returns>
        /// The same date/time with the UTC flag set
        /// </returns>
        public static DateTime AssumeUniversalTime(this DateTime dateTime)
        {
            return new DateTime(dateTime.Ticks, DateTimeKind.Utc);
        }

        /// <summary>
        /// Returns a copy of a nullable date/time value with its kind
        /// set to <see cref="DateTimeKind.Utc"/> but does not perform
        /// any time-zone adjustment.
        /// </summary>
        /// <remarks>
        /// This method is useful when obtaining date/time values from sources
        /// that might not correctly set the UTC flag.
        /// </remarks>
        /// <param name="dateTime">
        /// The nullable date/time
        /// </param>
        /// <returns>
        /// The same nullable date/time with the UTC flag set
        /// </returns>
        public static DateTime? AssumeUniversalTime(this DateTime? dateTime)
        {
            return dateTime.HasValue ? AssumeUniversalTime(dateTime.Value) : (DateTime?)null;
        }

        /// <summary>
        /// Converts a nullable UTC date/time value to local time.
        /// </summary>
        /// <param name="dateTime">
        /// The nullable UTC date/time
        /// </param>
        /// <returns>
        /// The nullable UTC date/time as local time
        /// </returns>
        public static DateTime? ToLocalTime(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToLocalTime() : (DateTime?)null;
        }

        /// <summary>
        /// Returns a date that is rounded to the next even hour above the given
        /// date.
        /// <p>
        /// For example an input date with a time of 08:13:54 would result in a date
        /// with the time of 09:00:00. If the date's time is in the 23rd hour, the
        /// date's 'day' will be promoted, and the time will be set to 00:00:00.
        /// </p>
        /// </summary>
        /// <param name="dateTime">
        /// the Date to round, if <see langword="null"/> the current time will
        /// be used
        /// </param>
        /// <returns>
        /// the new rounded date
        /// </returns>
        public static DateTime GetEvenHourDate(this DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                dateTime = DateTime.UtcNow;
            }

            DateTime d = dateTime.Value.AddHours(1);
            return new DateTime(d.Year, d.Month, d.Day, d.Hour, 0, 0);
        }

        /// <summary>
        /// Returns a date that is rounded to the next even minute above the given
        /// date.
        /// <p>
        /// For example an input date with a time of 08:13:54 would result in a date
        /// with the time of 08:14:00. If the date's time is in the 59th minute,
        /// then the hour (and possibly the day) will be promoted.
        /// </p>
        /// </summary>
        /// <param name="dateTime">
        /// The Date to round, if <see langword="null"/> the current time will  be used
        /// </param>
        /// <returns>
        /// The new rounded date
        /// </returns>
        public static DateTime GetEvenMinuteDate(this DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                dateTime = DateTime.UtcNow;
            }

            DateTime d = dateTime.Value;
            d = d.AddMinutes(1);
            return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0);
        }

        /// <summary>
        /// Returns a date that is rounded to the previous even minute below the
        /// given date.
        /// <p>
        /// For example an input date with a time of 08:13:54 would result in a date
        /// with the time of 08:13:00.
        /// </p>
        /// </summary>
        /// <param name="dateTime">
        /// the Date to round, if <see langword="null"/> the current time will
        /// be used
        /// </param>
        /// <returns>
        /// the new rounded date
        /// </returns>
        public static DateTime GetEvenMinuteDateBefore(this DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                dateTime = DateTime.UtcNow;
            }

            DateTime d = dateTime.Value;
            return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0);
        }

        /// <summary>
        /// Returns a date that is rounded to the next even second above the given
        /// date.
        /// </summary>
        /// <param name="dateTime">
        /// the Date to round, if <see langword="null"/> the current time will
        /// be used
        /// </param>
        /// <returns>
        /// the new rounded date
        /// </returns>
        public static DateTime GetEvenSecondDate(this DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                dateTime = DateTime.UtcNow;
            }

            DateTime d = dateTime.Value;
            d = d.AddSeconds(1);
            return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second);
        }

        /// <summary>
        /// Returns a date that is rounded to the previous even second below the
        /// given date.
        /// <p>
        /// For example an input date with a time of 08:13:54.341 would result in a
        /// date with the time of 08:13:00.000.
        /// </p>
        /// </summary>
        /// <param name="dateTime">
        /// the Date to round, if <see langword="null"/> the current time will
        /// be used
        /// </param>
        /// <returns>
        /// the new rounded date
        /// </returns>
        public static DateTime GetEvenSecondDateBefore(this DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                dateTime = DateTime.UtcNow;
            }

            DateTime d = dateTime.Value;
            return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second);
        }

        /// <summary>
        /// Returns a date that is rounded to the next even multiple of the given
        /// minute.
        /// <p>
        /// For example an input date with a time of 08:13:54, and an input
        /// minute-base of 5 would result in a date with the time of 08:15:00. The
        /// same input date with an input minute-base of 10 would result in a date
        /// with the time of 08:20:00. But a date with the time 08:53:31 and an
        /// input minute-base of 45 would result in 09:00:00, because the even-hour
        /// is the next 'base' for 45-minute intervals.
        /// </p>
        /// <p>
        /// More examples: <table>
        /// <tr>
        /// <th>Input Time</th>
        /// <th>Minute-Base</th>
        /// <th>Result Time</th>
        /// </tr>
        /// <tr>
        /// <td>11:16:41</td>
        /// <td>20</td>
        /// <td>11:20:00</td>
        /// </tr>
        /// <tr>
        /// <td>11:36:41</td>
        /// <td>20</td>
        /// <td>11:40:00</td>
        /// </tr>
        /// <tr>
        /// <td>11:46:41</td>
        /// <td>20</td>
        /// <td>12:00:00</td>
        /// </tr>
        /// <tr>
        /// <td>11:26:41</td>
        /// <td>30</td>
        /// <td>11:30:00</td>
        /// </tr>
        /// <tr>
        /// <td>11:36:41</td>
        /// <td>30</td>
        /// <td>12:00:00</td>
        /// </tr>
        /// <tr>
        /// <td>11:16:41</td>
        /// <td>17</td>
        /// <td>11:17:00</td>
        /// </tr>
        /// <tr>
        /// <td>11:17:41</td>
        /// <td>17</td>
        /// <td>11:34:00</td>
        /// </tr>
        /// <tr>
        /// <td>11:52:41</td>
        /// <td>17</td>
        /// <td>12:00:00</td>
        /// </tr>
        /// <tr>
        /// <td>11:52:41</td>
        /// <td>5</td>
        /// <td>11:55:00</td>
        /// </tr>
        /// <tr>
        /// <td>11:57:41</td>
        /// <td>5</td>
        /// <td>12:00:00</td>
        /// </tr>
        /// <tr>
        /// <td>11:17:41</td>
        /// <td>0</td>
        /// <td>12:00:00</td>
        /// </tr>
        /// <tr>
        /// <td>11:17:41</td>
        /// <td>1</td>
        /// <td>11:08:00</td>
        /// </tr>
        /// </table>
        /// </p>
        /// </summary>
        /// <param name="dateTime">
        /// the Date to round, if <see langword="null"/> the current time willbe used
        /// </param>
        /// <param name="minuteBase">
        /// the base-minute to set the time on
        /// </param>
        /// <returns>
        /// The new rounded date
        /// </returns>
        public static DateTime GetNextGivenMinuteDate(this DateTime? dateTime, int minuteBase)
        {
            if (minuteBase < 0 || minuteBase > 59)
            {
                throw new ArgumentException("minuteBase must be >=0 and <= 59");
            }

            if (!dateTime.HasValue)
            {
                dateTime = DateTime.UtcNow;
            }

            DateTime d = dateTime.Value;

            if (minuteBase == 0)
            {
                d = d.AddHours(1);
                return new DateTime(d.Year, d.Month, d.Day, d.Hour, 0, 0);
            }

            var minute = d.Minute;
            var itr = minute / minuteBase;
            var nextMinuteOccurance = minuteBase * (itr + 1);

            if (nextMinuteOccurance < 60)
            {
                return new DateTime(d.Year, d.Month, d.Day, d.Hour, nextMinuteOccurance, 0);
            }

            d = d.AddHours(1);
            return new DateTime(d.Year, d.Month, d.Day, d.Hour, 0, 0);
        }

        /// <summary>
        /// Returns a date that is rounded to the next even multiple of the given
        /// minute.
        /// <p>
        /// The rules for calculating the second are the same as those for
        /// calculating the minute in the method
        /// <see cref="GetNextGivenMinuteDate"/>.
        /// </p>
        /// </summary>
        /// <param name="dateTime">
        /// The date.
        /// </param>
        /// <param name="secondBase">
        /// The second base.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime GetNextGivenSecondDate(this DateTime? dateTime, int secondBase)
        {
            if (secondBase < 0 || secondBase > 59)
            {
                throw new ArgumentException("secondBase must be >=0 and <= 59");
            }

            if (!dateTime.HasValue)
            {
                dateTime = DateTime.UtcNow;
            }

            var d = dateTime.Value;

            if (secondBase == 0)
            {
                d = d.AddMinutes(1);
                return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0);
            }

            var second = d.Second;
            var itr = second / secondBase;
            var nextSecondOccurance = secondBase * (itr + 1);

            if (nextSecondOccurance < 60)
            {
                return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, nextSecondOccurance);
            }

            d = d.AddMinutes(1);
            return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0);
        }

        /// <summary>
        /// Translate a date and time from a users timezone to the another
        /// (probably server) timezone to assist in creating a simple trigger with
        /// the right date and time.
        /// </summary>
        /// <param name="date">
        /// the date to translate
        /// </param>
        /// <param name="src">
        /// the original time-zone
        /// </param>
        /// <param name="dest">
        /// the destination time-zone
        /// </param>
        /// <returns>
        /// the translated UTC date
        /// </returns>
        public static DateTime TranslateTime(this DateTime date, TimeZoneInfo src, TimeZoneInfo dest)
        {
            var newDate = DateTime.UtcNow;
            var offset = GetOffset(date, dest) - GetOffset(date, src);

            newDate = newDate.AddMilliseconds(-1 * offset);

            return newDate;
        }

        /// <summary>
        /// Gets the offset from UT for the given date in the given timezone,
        /// taking into account daylight savings.
        /// </summary>
        /// <param name="date">
        /// the date that is the base for the offset
        /// </param>
        /// <param name="tz">
        /// the time-zone to calculate to offset to
        /// </param>
        /// <returns>
        /// the offset
        /// </returns>
        public static double GetOffset(this DateTime date, TimeZoneInfo tz)
        {
            if (tz.IsDaylightSavingTime(date))
            {
                return tz.BaseUtcOffset.TotalMilliseconds + 0;
            }

            return tz.BaseUtcOffset.TotalMilliseconds;
        }

        /// <summary>
        /// This functions determines if the TimeZone uses daylight saving time
        /// </summary>
        /// <param name="timezone">
        /// TimeZone instance to validate
        /// </param>
        /// <returns>
        /// True or false depending if daylight savings time is used
        /// </returns>
        public static bool UseDaylightTime(this TimeZoneInfo timezone)
        {
            return timezone.SupportsDaylightSavingTime;
        }

        /// <summary>
        /// The to java script ticks.
        /// </summary>
        /// <param name="dateTime">
        /// The date time.
        /// </param>
        /// <returns>
        /// The <see cref="long"/>.
        /// </returns>
        public static long ToJavaScriptTicks(this DateTime dateTime)
        {
            DateTimeOffset utcDateTime = dateTime.ToUniversalTime();
            var javaScriptTicks = (utcDateTime.Ticks - BeginOfEpoch.Ticks) / 10000;
            return javaScriptTicks;
        }

        /// <summary>
        /// The to serialization mode.
        /// </summary>
        /// <param name="kind">
        /// The kind.
        /// </param>
        /// <returns>
        /// The <see cref="XmlDateTimeSerializationMode"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 超出范围异常
        /// </exception>
        public static XmlDateTimeSerializationMode ToSerializationMode(DateTimeKind kind)
        {
            switch (kind)
            {
                case DateTimeKind.Local:
                    return XmlDateTimeSerializationMode.Local;

                case DateTimeKind.Unspecified:
                    return XmlDateTimeSerializationMode.Unspecified;

                case DateTimeKind.Utc:
                    return XmlDateTimeSerializationMode.Utc;

                default:
                    throw new ArgumentOutOfRangeException("kind", kind, "Unexpected DateTimeKind value.");
            }
        }

        /// <summary>
        /// Get the first day of the month for
        /// any full date submitted
        /// </summary>
        /// <param name="date">
        /// 日期
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime GetFirstDayOfMonth(this DateTime date)
        {
            var @from = date;
            @from = @from.AddDays(-(@from.Day - 1));
            return @from;
        }

        /// <summary>
        /// Get the last day of the month for any
        /// full date
        /// </summary>
        /// <param name="date">
        /// 日期
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime GetLastDayOfMonth(this DateTime date)
        {
            var to = date;
            to = to.AddMonths(1);
            to = to.AddDays(-to.Day);
            return to;
        }

        /// <summary>
        /// The to start of the day.
        /// 设置一天最后的时分秒  00:00:00
        /// </summary>
        /// <param name="dt">
        /// The dt.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime ToStartOfTheDay(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
        }


        /// <summary>
        /// The to end of the day.
        /// 设置一天最后的时分秒  00:00:00
        /// </summary>
        /// <param name="dt">
        /// The dt.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>DateTime?</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static DateTime? ToStartOfTheDay(this DateTime? dt)
        {
            return dt.HasValue ? dt.Value.ToStartOfTheDay() : dt;
        }

        /// <summary>
        /// The to end of the day.
        /// 设置一天最后的时分秒  23:59:59
        /// </summary>
        /// <param name="dt">
        /// The dt.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime ToEndOfTheDay(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
        }

        /// <summary>
        /// The to end of the day.
        /// 设置一天最后的时分秒  23:59:59
        /// </summary>
        /// <param name="dt">
        /// The dt.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>DateTime?</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static DateTime? ToEndOfTheDay(this DateTime? dt)
        {
            return dt.HasValue ? dt.Value.ToEndOfTheDay() : dt;
        }

        /// <summary>
        /// Epoch time. Number of seconds since midnight (UTC) on 1st January 1970.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see cref="long"/>.
        /// </returns>
        public static long ToUnixTime(this DateTime date)
        {
            return Convert.ToInt64((date.ToUniversalTime() - BeginOfEpoch).TotalSeconds);
        }

        /// <summary>
        /// UTC date based on number of seconds since midnight (UTC) on 1st January 1970.
        /// </summary>
        /// <param name="unixTime">
        /// The unix Time.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime FromUnixTime(this long unixTime)
        {
            return BeginOfEpoch.AddSeconds(unixTime);
        }
    }
}