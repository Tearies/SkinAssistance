using System;
using System.Windows;

namespace SkinAssistance.Core.Extensions
{
    public class DurationBuilder
    {
        /// <summary>
        /// TimeSpan 转Duration
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        /// <returns></returns>
        public static Duration FromTimeSpan(TimeSpan timeSpan)
        {
            return new Duration(timeSpan);
        }

        public static Duration FromSeconds(double seconds)
        {
            return FromTimeSpan(TimeSpan.FromSeconds(seconds));
        }

        public static Duration FromMilliseconds(double milliseconds)
        {
            return FromTimeSpan(TimeSpan.FromMilliseconds(milliseconds));
        }

        public static Duration FromTicks(long ticks)
        {
            return FromTimeSpan(TimeSpan.FromTicks(ticks));
        }
    }
}