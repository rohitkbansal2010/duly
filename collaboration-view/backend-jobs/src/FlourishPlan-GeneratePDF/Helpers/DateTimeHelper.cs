using System;

namespace Duly.UI.Flourish.GeneratePDF.Helpers
{
    public static class DateTimeHelper
    {
        public static string AddZeroIfOneDigit(int num)
        {
            return (num) < 10 ? $"0{num}" : $"{num}";
        }

        public static string ConvertTo12HourFormat(string time)
        {
            var timeArray = time.Split(':');
            var hours = Int32.Parse(timeArray[0]);
            var minutes = Int32.Parse(timeArray[1]);

            var ampm = hours >= 12 ? "PM" : "AM";
            var newHours = AddZeroIfOneDigit(hours % 12);
            var newMinutes = AddZeroIfOneDigit(minutes);

            if (newHours == "00")
            {
                return $"12:{newMinutes} { ampm}";
            }
            return $"{newHours}:{ newMinutes} { ampm}";
        }
    }
}