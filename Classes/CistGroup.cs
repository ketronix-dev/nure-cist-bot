namespace NureCistBot.Classes;

public class CistGroup
{
    public CistUniversity? university;
}

public class CistUniversity
{
    public string? short_name { get; set; }
    public string? full_name { get; set; }
    public CistFaculty[]? faculties { get; set; }
}

public class CistFaculty
{
    public long? id { get; set; }
    public string? short_name { get; set; }
    public string? full_name { get; set; }
    public Direction[]? directions { get; set; }
}

public class Direction
{
    public long? id { get; set; }
    public string? short_name { get; set; }
    public string? full_name { get; set; }
    public Specialition[]? specialities { get; set; }
    public Group[]? groups { get; set; }
}

public class Specialition
{
    public long? id { get; set; }
    public string? short_name { get; set; }
    public string? full_name { get; set; }
    public Group[]? groups { get; set; }
}