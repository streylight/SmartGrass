using System;

namespace Core.Helpers {
    /// <summary>
    /// Datetime helper
    /// </summary>
    public static class DateTimeHelper {
        /// <summary>
        /// Return CST *for demo purposes only
        /// </summary>
        /// <returns></returns>
        public static DateTime GetLocalTime() {
            var cstZone = TimeZoneInfo.FindSystemTimeZoneById( "Central Standard Time" );
            return TimeZoneInfo.ConvertTimeFromUtc( DateTime.UtcNow, cstZone );
        }
    } // class
} // namespace
