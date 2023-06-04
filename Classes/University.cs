namespace NureCistBot.Classes;

public class University
{
    public string? short_name { get; set; }
    public string? full_name { get; set; }
    public Faculty[]? faculties { get; set; }
}

public class Faculty
{
    public long? id { get; set; }
    public string? short_name { get; set; }
    public string? full_name { get; set; }
    public Departament[]? departaments { get; set; }
}

public class Departament
{
    public long? id { get; set; }
    public string? short_name { get; set; }
    public string? full_name { get; set; }
    public Teacher[]? teachers { get; set; }
}