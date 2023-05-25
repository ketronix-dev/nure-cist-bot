using Telegram.BotAPI;
using Telegram.BotAPI.GettingUpdates;
using Newtonsoft.Json;

namespace NureCistBot.Handlers
{
    public class ExceptionHandler
    {
        public static void SendToLogs(BotClient bot, Update update)
        {
            Console.WriteLine($"Update ID: {update.UpdateId}");
            Console.WriteLine($"Update type: {update.Type}");
            Console.WriteLine($"Update stack: {JsonConvert.SerializeObject(update.Message, Formatting.Indented)}");
        }
    }
}