using Newtonsoft.Json;
using NureCistBot.Classes;

namespace NureCistBot.JsonParsers;

public class GroupsParser
{
    public static List<Group> Parse()
    {
        List<Group> groups = new List<Group>();

        string json = File.ReadAllText("/home/artem/Documents/groups.json");
        CistGroup? cistGroups = JsonConvert.DeserializeObject<CistGroup>(json);
        
        Console.WriteLine(cistGroups.university.faculties);

        foreach (var faculty in cistGroups.university.faculties)
        {
            foreach (var direction in faculty.directions)
            {
                foreach (var specialition in direction.specialities)
                {
                    foreach (var group in specialition.groups)
                    {
                        groups.Add(group);
                    }
                }
            }
        }

        return groups;
    }
}