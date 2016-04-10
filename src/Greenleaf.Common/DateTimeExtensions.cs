using System;

namespace Greenleaf
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime _unixStart = new DateTime(1970, 1, 1);
        
        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            return (long)(dateTime - _unixStart).TotalMilliseconds;
        } 
    }
}