using NureCistBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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
                        await bot.SendTextMessageAsync(
                            ChatId,
                            "Цей бот має низку команд, за допомогою яких ви можете отримати розклад для себе, " +
                            "і своєї групи. Нижче буде список цих команд, із коротким описом, і прикладом. \n \n" +
                            "Список команд бота: \n \n" +
                            "\t <code>/chgroup group</code> - зміна групи у чаті, замість group треба написати назву вашої групи. " +
                            "Наприклад: <code>/chgroup КІУКІ-22-7</code>, <code>/chgroup кіукі-22-7</code> і тд. Увага! Назву групи бот розуміє лише " +
                            "якщо та була введена українською, через те шо він звіряє назву із реєстром на сайті cist.nure.ua." +
                            " Якщо у вас виникла помилка зміни групи, перевірте щоб назва була українською мовою, " +
                            "і відповідала тій що на cist.nure.ua. \n" +
                            "\t <code>/help</code> - вам відправиться це повідомлення. \n" +
                            "\t <code>/info</code> - вам відправиться інформація про цей чат із бази даних, якщо запис існує. \n" +
                            "\t <code>/day</code> - вам відправиться розклад для вашої групи на поточний день. \n" +
                            "\t <code>/week</code> - вам відправиться розклад для вашої групи на поточний тиждень. " +
                            "У неділю ця команда вам відправить розклад вже на наступний тиждень. \n" +
                            "\t <code>/next_day</code> - відправляє розклад на наступний день. \n" +
                            "\t <code>/next_week</code> - відправить розклад на наступний тиждень. \n \n" +
                            "<b>УВАГА!</b> Якщо ви зробите запит на розклад вперше за добу - то підготовка розкладу буде тривати " +
                            " приблизно 11 секунд. Подякувати за це можете розробникам CISTу.",
                            parseMode: ParseMode.Html);
                    }
                }
            }

        }
    }
}