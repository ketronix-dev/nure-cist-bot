using System.Globalization;

namespace NureCistBot.DateManagment;

public class DateService
{
    public static DateOnly[] GetWeekDates(DateOnly date)
    {
        var days = new DateOnly[2];
        var currentDate = date.AddDays(-(int)date.DayOfWeek + (int)DayOfWeek.Monday);
        days[0] = currentDate;
        currentDate = currentDate.AddDays(5);
        days[1] = currentDate;
        return days;
    }

    public static DateOnly[] GetNextWeekDates(DateOnly date)
    {
        var days = new DateOnly[2];
        var currentDate = date.AddDays(7 - (int)date.DayOfWeek + (int)DayOfWeek.Monday);
        days[0] = currentDate;
        currentDate = currentDate.AddDays(5);
        days[1] = currentDate;
        return days;
    }


    public static DateOnly GetToday() => new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
    public static DateOnly GetNextDay() => new DateOnly(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day);

    public static string[] GetWeekDays(string startDateString, string endDateString)
    {
        DateTime startDate = DateTime.ParseExact(startDateString, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        DateTime endDate = DateTime.ParseExact(endDateString, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        int daysDifference = (endDate - startDate).Days;

        if (daysDifference != 5 || startDate.DayOfWeek != DayOfWeek.Monday || endDate.DayOfWeek != DayOfWeek.Saturday)
        {
            throw new ArgumentException("The input dates are not a valid week");
        }

        string[] dates = new string[6];
        for (int i = 0; i < 6; i++)
        {
            dates[i] = startDate.AddDays(i).ToString("dd.MM.yyyy");
        }

        return dates;
    }

}