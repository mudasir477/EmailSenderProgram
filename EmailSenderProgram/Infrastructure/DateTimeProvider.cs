using System;

namespace EmailSenderProgram.Extensions.Services
{
    public static class DateTimeProvider
    {
        public static bool IsSunday()
        {

            return DateTime.Now.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}
