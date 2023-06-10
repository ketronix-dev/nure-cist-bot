using NureCistBot.Classes;
namespace NureCistBot.Services
{
    public class GroupServices
    {
        public static Group? FindGroupByName(List<Group> groups, string name)
        {
            foreach (Group group in groups)
            {
                if (group.Name.ToUpper() == name)
                {
                    return group;
                }
            }

            return null;
        }

    }
}