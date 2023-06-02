using NureCistBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NureCistBot.Handlers
{
    public class CallbackHandler
    {
        public static async Task HandleCallbackAsync(ITelegramBotClient bot, CallbackQuery? callback, long ChatId)
        {
            var isAdmin = await BotService.IsAdminAsync(bot, ChatId);
            if (callback is not null)
            {
                if (callback.Data == "CheckAdmin")
                {
                    if (!isAdmin)
                    {
                        await bot.AnswerCallbackQueryAsync(callback.Id, "Бот не є адміном");
                    }
                    else
                    {
                        await bot.SendTextMessageAsync(ChatId, "Help message");
                    }
                }
            }

        }
    }
}