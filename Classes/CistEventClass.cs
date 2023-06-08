namespace NureBotSchedule.ServiceClasses;

public class CistEvent
{
    public int number_pair { get; set; }
    public Subject subject { get; set; }
    public DateOnly date { get; set; }
    public TimeOnly start_time { get; set; }
    public TimeOnly end_time { get; set; }
    public string type { get; set; }
    public List<Teacher> teachers { get; set; }
    
}