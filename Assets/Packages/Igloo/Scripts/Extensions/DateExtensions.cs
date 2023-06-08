using System;

namespace Packages.Igloo.Scripts.Extensions
{
    public static class DateExtensions
    {
        public static string FormatDate(this TimeSpan timeSpan)
        {
            string formattedTimeSpan = "";
            if (timeSpan.Days > 0)
            {
                formattedTimeSpan += timeSpan.Days + "d ";
                formattedTimeSpan += timeSpan.Hours + "h";
            }
            else if (timeSpan.Hours > 0)
            {
                formattedTimeSpan += timeSpan.Hours + "h ";
                formattedTimeSpan += timeSpan.Minutes + "m";
            }
            else
            {
                formattedTimeSpan += timeSpan.Minutes + "m ";
                formattedTimeSpan += timeSpan.Seconds + "s";
            }

            return formattedTimeSpan;
        }
    }
}
