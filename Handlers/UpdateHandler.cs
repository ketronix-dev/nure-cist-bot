using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.GettingUpdates;

namespace NureCistBot.Handlers
{
    public class UpdateHandler
    {
        public static void HandleUpdate(BotClient bot, Update update)
        {
            var message = update.Message;
            if (message is not null)
            {
                bot.SendChatAction(message.Chat.Id, ChatAction.UploadDocument);
                Thread.Sleep(5000);
                bot.SendMessage(message.Chat.Id, "Hello World!");
            }
        }
    }
}