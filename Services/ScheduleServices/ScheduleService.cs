using NureCistBot.Classes;

namespace NureCistBot.Services;

public class ScheduleService
{
    public static List<CistEvent> GetCistShedule(DbChat chat, string Key)
    {
        var result = NureCistParser.Parse(chat.CistId, Key);
        return result;
    }

    public static List<CistEvent> GetCistEvents(List<CistEvent> events, DateOnly startDate, DateOnly endDate)
    {
        return events.Where(e => e.date >= startDate && e.date <= endDate).ToList();
    }
}