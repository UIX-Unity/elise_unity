using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static partial class Helper
{
    public static class Date
    {
        /// <summary>
        /// <para>Compare(8/7/2019 6:10:00 AM , 8/7/2019 6:50:00 AM) = +1     # 오른쪽 날짜가 더 최근 날짜인 경우</para>
        /// <para>Compare(8/7/2019 6:10:00 AM , 8/7/2019 6:10:00 AM) =  0     # 날짜가 같은 경우</para>
        /// <para>Compare(8/7/2019 6:50:00 AM , 8/7/2019 6:10:00 AM) = -1     # 왼쪽 날짜가 더 최근 날짜인 경우</para>
        /// </summary>
        public static int DateCompare(string dateFormat, string date1, string date2)
        {
            DateTime leftDate;
            DateTime.TryParseExact
                (date1
                , dateFormat
                , null
                , System.Globalization.DateTimeStyles.AllowWhiteSpaces
                | System.Globalization.DateTimeStyles.AdjustToUniversal
                , out leftDate);

            DateTime rightDate;
            DateTime.TryParseExact
                (date2
                , dateFormat
                , null
                , System.Globalization.DateTimeStyles.AllowWhiteSpaces
                | System.Globalization.DateTimeStyles.AdjustToUniversal
                , out rightDate);

            /*

            Compare(8/7/2019 6:10:00 AM , 8/7/2019 6:50:00 AM) = +1     # 오른쪽 날짜가 더 최근 날짜인 경우
            Compare(8/7/2019 6:10:00 AM , 8/7/2019 6:10:00 AM) =  0     # 날짜가 같은 경우
            Compare(8/7/2019 6:50:00 AM , 8/7/2019 6:10:00 AM) = -1     # 왼쪽 날짜가 더 최근 날짜인 경우

            */

            return DateTime.Compare(leftDate, rightDate);
        }

        public static TimeSpan GetRemainDate(string dateFormat, string date1, string date2)
        {
            DateTime leftDate;
            DateTime.TryParseExact
                (date1
                , dateFormat
                , null
                , System.Globalization.DateTimeStyles.AllowWhiteSpaces
                | System.Globalization.DateTimeStyles.AdjustToUniversal
                , out leftDate);

            DateTime rightDate;
            DateTime.TryParseExact
                (date2
                , dateFormat
                , null
                , System.Globalization.DateTimeStyles.AllowWhiteSpaces
                | System.Globalization.DateTimeStyles.AdjustToUniversal
                , out rightDate);

            return leftDate - rightDate;
        }

        public static TimeSpan GetRemainDate(DateTime startDateTime, DateTime endDateTime)
        {
            return endDateTime - startDateTime;
        }

        public static double GetRemainHours(DateTime startDateTime, DateTime endDateTime)
        {
            var timespan = GetRemainDate(startDateTime, endDateTime);
            return timespan.TotalHours;
        }
        public static double GetRemainMinutes(DateTime startDateTime, DateTime endDateTime)
        {
            var timespan = GetRemainDate(startDateTime, endDateTime);
            return timespan.TotalMinutes;
        }
        public static double GetRemainSeconds(DateTime startDateTime, DateTime endDateTime)
        {
            var timespan = GetRemainDate(startDateTime, endDateTime);
            return timespan.TotalSeconds;
        }
        public static double GetRemainMilliseconds(DateTime startDateTime, DateTime endDateTime)
        {
            var timespan = GetRemainDate(startDateTime, endDateTime);
            return timespan.TotalMilliseconds;
        }

        public static bool TryParseToDateTime(string dateFormat, string date, out DateTime datetime)
        {
            return DateTime.TryParseExact
                (date
                , dateFormat
                , null
                , System.Globalization.DateTimeStyles.AllowWhiteSpaces
                | System.Globalization.DateTimeStyles.AdjustToUniversal
                , out datetime);
        }
    }

}
