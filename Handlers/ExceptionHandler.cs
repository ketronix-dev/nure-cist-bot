using Newtonsoft.Json;
using Telegram.BotAPI;
using Telegram.BotAPI.GettingUpdates;

namespace NureCistBot.Handlers
{
    public class ExceptionHandler
    {
        public static void SendToLogs(BotClient bot, Update update, Exception exception)
        {
            Console.WriteLine($"Update ID: {update.UpdateId}");
            Console.WriteLine($"Update type: {update.Type}");
            Console.WriteLine($"Update stack: {JsonConvert.SerializeObject(update.Message, Formatting.Indented)}");
            Console.WriteLine($"Exception: {JsonConvert.SerializeObject(exception, Formatting.Indented)}");
        }
    }
}