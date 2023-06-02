using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;

namespace NureCistBot.Services
{
    public class BotService
    {
        public static bool IsAdmin(BotClient bot, long chatId)
        {
            var members = bot.GetChatAdministrators(chatId);
            foreach (var member in members)
            {
                if (member.User == bot.GetMe())
                {
                    return true;
                }
            }
            return false;
        }
    }
}