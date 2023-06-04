namespace NureCistBot.Classes;

public class Schedule
{
    public string? time_zone { get; set; }
    public Event[]? events { get; set; }
    public Teacher[]? teachers { get; set; }
    public Subject[]? subjects { get; set; }
}

public class Event
{
    public int? subject_id { get; set; }
    public long? start_time { get; set; }
    public long? end_time { get; set; }
    public int? type { get; set; }
    public int? number_pair { get; set; }
    public string? auditory { get; set; }
    public int[]? teachers { get; set; }
    public int[]? groups { get; set; }
}

public class Teacher
{
    public int? id { get; set; }
    public string? full_name { get; set; }
    public string? short_name { get; set; }
}

public class Subject
{
    public int? id { get; set; }
    public string? title { get; set; }
    public string? brief { get; set; }
}