using System;

namespace Pds.FundingClaim.Services.Extensions
{
    /// <summary>
    /// Extension methods on string.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts the string to utc date time.
        /// </summary>
        /// <param name="dateTimeAsString">The string to act on.</param>
        /// <returns>The converted datetime.</returns>
        public static DateTime ToUtcDateTime(this string dateTimeAsString)
        {
            return DateTimeOffset.Parse(dateTimeAsString).UtcDateTime;
        }
    }
}