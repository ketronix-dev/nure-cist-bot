using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace NureCistBot.Services
{
    public class BotService
    {
        public static async Task<bool> IsAdminAsync(ITelegramBotClient bot, long chatId)
        {
            try
            {
                var me = await bot.GetMeAsync();
                var chatMembers = await bot.GetChatAdministratorsAsync(chatId);
                return chatMembers.Any(x => x.User.Id == me.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured while checking bot admin status: " + ex.Message);
                return false;
            }
        }
    }
}