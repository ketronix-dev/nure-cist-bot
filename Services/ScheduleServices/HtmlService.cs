using Firebase.Database;
using Firebase.Database.Query;
using NureCistBot.Classes;

namespace NureCistBot.Services;

public class HtmlService
{
    public static FirebaseClient firebase = new FirebaseClient(
        "https://schedulebot-ea3d4-default-rtdb.europe-west1.firebasedatabase.app/");

    public static async Task<string>? SubjectLink(string GroupId, CistEvent cistEvent, bool eng = false)
    {
        string? link;
        if (eng == false)
        {
            link = await firebase
                .Child($"Links/{GroupId}/{cistEvent.subject.brief}/{cistEvent.type}")
                .OrderByKey()
                .OnceSingleAsync<string>();

        }
        else
        {
            link = await firebase
                .Child($"Links/{GroupId}/{cistEvent.subject.brief}/{cistEvent.teachers[0].id}/{cistEvent.type}")
                .OrderByKey()
                .OnceSingleAsync<string>();

        }
        if (link is not null && link is not "")
        {
            return link;
        }
        else
        {
            return null;
        }
    }

    public static string GetEventHtml(CistEvent i, string GroupId)
    {
        var message = "";
        string link;
        if (i.subject.brief == "ІМ")
        {
            link = SubjectLink(GroupId, i, true).Result;
        }
        else
        {
            link = SubjectLink(GroupId, i).Result;
        }
        if (link is null || link == "")
        {
            message = $"{i.start_time.ToString("HH:mm")}-{i.end_time.ToString("HH:mm")} | <b>{i.subject.brief} - {i.type.ToUpper()}</b> |" +
                      $"\t <a href=\"\">{i.teachers[0].short_name}</a>\n";
            return message;
        }
        message = $"{i.start_time.ToString("HH:mm")}-{i.end_time.ToString("HH:mm")} | <b>{i.subject.brief} - {i.type.ToUpper()}</b> |" +
                  $"\t <a href=\"{link}\">{i.teachers[0].short_name}</a>\n";

        return message;
    }
}
