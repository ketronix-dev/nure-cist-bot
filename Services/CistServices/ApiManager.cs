using System.Net;
using System.Text;
using NureCistBot.JsonParsers;

namespace NureCistBot.CistServices;

public class ApiManager
{
    public static DateOnly DueDate;
    public static string CheckOrCreateFile(long Id, string Key)
    {
        string day = $"{DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year}";
        string PrevDay = $"{DateTime.Today.AddDays(-1).Day}.{DateTime.Today.AddDays(-1).Month}.{DateTime.Today.AddDays(-1).Year}";

        if (File.Exists($"Cache/schedule-{Id}-{day}.json"))
        {
            if (File.Exists($"Cache/schedule-{Id}-{PrevDay}.json"))
            {
                File.Delete($"Cache/schedule-{Id}-{PrevDay}.json");
            }
            return $"Cache/schedule-{Id}-{day}.json";
        }
        else
        {
            Directory.CreateDirectory("Cache");
            Thread.Sleep(4000);
            var webRequest = WebRequest.Create($"https://cist.nure.ua/ias/app/tt/P_API_EVENT_JSON?timetable_id={Id}&idClient={Key}") as HttpWebRequest;

            webRequest.ContentType = "application/json";
            webRequest.UserAgent = "KIUKI_Bot";

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (var webResponse = webRequest.GetResponse())
            using (var streamReader = new StreamReader(webResponse.GetResponseStream(), Encoding.GetEncoding("windows-1251")))
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            {
                streamWriter.Write(streamReader.ReadToEnd());
                streamWriter.Flush();
                memoryStream.Position = 0;

                var json = JsonFixers.TryFix(Encoding.UTF8.GetString(memoryStream.ToArray()));
                File.WriteAllBytes($"Cache/schedule-{Id}-{day}.json", Encoding.UTF8.GetBytes(json));
            }

            return $"Cache/schedule-{Id}-{day}.json";
        }
    }

    public static string UpdateGroupsList()
    {
        DateOnly day = DateOnly.FromDateTime(DateTime.Now);
        DateOnly NextDay = DateOnly.FromDateTime(DateTime.Now).AddDays(3);

        int due = DueDate.DayNumber - day.DayNumber;

        if (due <= 0)
        {
            Directory.CreateDirectory("Cache");
            Thread.Sleep(4000);
            var webRequest = WebRequest.Create("https://cist.nure.ua/ias/app/tt/P_API_GROUP_JSON") as HttpWebRequest;

            webRequest.ContentType = "application/json";
            webRequest.UserAgent = "KIUKI_Bot";

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (var webResponse = webRequest.GetResponse())
            using (var streamReader = new StreamReader(webResponse.GetResponseStream(), Encoding.GetEncoding("windows-1251")))
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            {
                streamWriter.Write(streamReader.ReadToEnd());
                streamWriter.Flush();
                memoryStream.Position = 0;

                var json = Encoding.UTF8.GetString(memoryStream.ToArray());
                File.WriteAllBytes("Cache/groups.json", Encoding.UTF8.GetBytes(json));
            }
            DueDate = NextDay;
            return "Cache/groups.json";
        }
        else
        {
            return "Cache/groups.json";
        }
    }



}