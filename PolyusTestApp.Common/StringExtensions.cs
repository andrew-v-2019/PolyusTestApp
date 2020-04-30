using System;

namespace PolyusTestApp.Common
{
    public static class StringExtensions
    {
        public static string FormatDateForFileName(this DateTime date)
        {
            return date.ToString().Replace(".", "_").Replace(":", "_");
        }
    }
}
