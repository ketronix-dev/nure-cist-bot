using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NureCistBot.Handlers
{
    public class ExceptionHandler
    {
        public static void SendToLogs(ITelegramBotClient bot, Update update, Exception exception)
        {
            Console.WriteLine($"Update ID: {update.Id}");
            Console.WriteLine($"Update type: {update.Type}");
            Console.WriteLine($"Update stack: {JsonConvert.SerializeObject(update.Message, Formatting.Indented)}");
            Console.WriteLine($"Exception: {JsonConvert.SerializeObject(exception, Formatting.Indented)}");
        }
    }
}