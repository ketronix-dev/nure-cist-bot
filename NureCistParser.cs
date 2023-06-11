using Newtonsoft.Json;
using NureCistBot.CistServices;
using NureCistBot.Classes;
using NureCistBot.JsonParsers;

public class NureCistParser
{
    public static List<CistEvent>? Parse(long Id, string Key)
    {
        try
        {
            CistEvent[]? events;

            string json = File.ReadAllText(ApiManager.CheckOrCreateFile(Id, Key));
            Schedule? schedule;
            try
            {
                schedule = JsonConvert.DeserializeObject<Schedule>(json);
            }
            catch (Exception e)
            {
                schedule = null;
            }
            
            events = EventComposer.GetEvents(schedule);

            if (events is not null)
            {
                return events.ToList().OrderBy
                        (@event => @event.date.ToDateTime(@event.start_time))
                    .ToList();
            }
            else
            {
                return null;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}