using NureBotSchedule.ServiceClasses;

namespace NureBotSchedule.Services.ScheduleServices;

public class Schedule
{
    public static List<CistEvent> GetCistShedule(Group group, string Key)
    {
        var result = NureCistParser.Parse(group.GroupId, Key);
        return result;
    }
    
    public static List<CistEvent> GetCistEvents(List<CistEvent> events, DateOnly startDate, DateOnly endDate)
    {
        return events.Where(e => e.date >= startDate && e.date <= endDate).ToList();
    }
}