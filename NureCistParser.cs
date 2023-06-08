using Newtonsoft.Json;
using NureCistBot.CistServices;
using NureCistBot.Classes;

public class NureCistParser
{
    public static List<CistEvent> Parse(long Id, string Key)
    {
        List<CistEvent> events = new List<CistEvent>();

        string json = File.ReadAllText(ApiManager.CheckOrCreateFile(Id, Key));
        Schedule schedule = JsonConvert.DeserializeObject<Schedule>(json);
        events = EventComposer.GetEvents(schedule).ToList();

        return events.OrderBy
                (@event => @event.date.ToDateTime(@event.start_time))
            .ToList();
    }
}