using Firebase.Database;
using Firebase.Database.Query;
using NureBotSchedule.ServiceClasses;

namespace NureBotSchedule.Services.ScheduleServices;

public class HtmlService
{
    public static FirebaseClient firebase = new FirebaseClient(
        "https://schedulebot-ea3d4-default-rtdb.europe-west1.firebasedatabase.app/");

    public static async Task<string>? SubjectLink(string GroupNumber, CistEvent cistEvent, bool eng = false)
    {
        string? link;
        if (eng == false)
        {
            link = await firebase
                .Child($"Links/{GroupNumber}/{cistEvent.subject.brief}/{cistEvent.type}")
                .OrderByKey()
                .OnceSingleAsync<string>();

        }
        else
        {
            link = await firebase
                .Child($"Links/{GroupNumber}/{cistEvent.subject.brief}/{cistEvent.teachers[0].id}/{cistEvent.type}")
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

    public static string GetEventHtml(CistEvent i, string GroupNumber)
    {
        var message = "";
        string link;
        if (i.subject.brief == "ІМ")
        {
            link = SubjectLink(GroupNumber, i, true).Result;
            Console.WriteLine($"Номер пари: {i.number_pair} \n" +
                              $"Предмет: {i.subject.brief} \n" +
                              $"Дата: {i.date} \n" +
                              $"Старт: {i.start_time} \n" +
                              $"Кінець: {i.end_time} \n" +
                              $"Тип: {i.type} \n" +
                              $"Викладач: {i.teachers[0].short_name} \n" +
                              $"Викладач ID: {i.teachers[0].id} \n" +
                              $"Link: {link} \n" +
                              "-----------------\n");
        }
        else
        {
            link = SubjectLink(GroupNumber, i).Result;
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
