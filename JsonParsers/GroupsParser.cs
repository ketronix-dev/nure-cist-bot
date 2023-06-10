using Newtonsoft.Json;
using NureCistBot.CistServices;
using NureCistBot.Classes;

namespace NureCistBot.JsonParsers;

public class GroupsParser
{
    public static List<Group> Parse()
    {
        List<Group> groups = new List<Group>();

        string json = File.ReadAllText(ApiManager.UpdateGroupsList());
        CistGroup? cistGroups = JsonConvert.DeserializeObject<CistGroup>(json);

        if (cistGroups is not null && cistGroups.university is not null)
        {
            if (cistGroups.university.faculties is not null)
            {
                foreach (var faculty in cistGroups.university.faculties)
                {
                    if (faculty.directions is not null)
                    {
                        foreach (var direction in faculty.directions)
                        {
                            if (direction.specialities is not null)
                            {
                                foreach (var specialition in direction.specialities)
                                {
                                    if (specialition.groups is not null)
                                    {
                                        foreach (var group in specialition.groups)
                                        {
                                            groups.Add(group);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        return groups;
    }
}