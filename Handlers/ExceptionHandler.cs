using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace NureCistBot.Handlers
{
    public class ExceptionHandler
    {
        public static void SendToLogs(ITelegramBotClient bot, Update update, Exception exception)
        {
            bot.SendTextMessageAsync(-1001638301850,$"Update ID: <b>{update.Id}</b>" +
                                                    $"Update type: <b>{update.Type}</b?" + 
                                                    $"Update stack: <code>{JsonConvert.SerializeObject(update.Message, Formatting.Indented)}</code>" +
                                                    $"Exception: <code>{JsonConvert.SerializeObject(exception, Formatting.Indented)}</code>",
                                      parseMode: ParseMode.Html);
        }
    }
}