using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace NureCistBot.Handlers
{
    public class NewMemberHandler
    {
        public static async Task HandleNewMember(ITelegramBotClient bot, User[] users, long ChatId)
        {
            var me = await bot.GetMeAsync();
            foreach (var user in users)
            {
                if (user.Id == me.Id)
                {
                    InlineKeyboardMarkup inlineKeyboard = new(new[]
                            {
                                InlineKeyboardButton.WithCallbackData(text: "Перевірити дозволи", callbackData: "CheckAdmin"),
                            });
                    await bot.SendChatActionAsync(ChatId, ChatAction.Typing);
                    UpdateHandler.joinMessage = await bot.SendTextMessageAsync(
                        ChatId,
                        "Привіт! Щоб бот отримав доступ до ваших повідомлень," +
                        " і міг реагувати на ваші команди - йому треба видати права адміністратора. \n \n" +
                        "Для того щоб перевірити, чи отримав їх бот - натисніть кнопку нижче." +
                        " У випадку не отримання адміністративних повноважень, перед вами з'явиться повідомлення " +
                        "з наступним текстом \"Бот не є адміном\", а після успішного отримання потрібних прав " +
                        "Буде відправлено повідомлення з усіма командами бота, котрі доступні користувачам. \n \n" +
                        "У випадку виникнення якихось помилок, ОБОВ'ЯЗКОВО напишіть <a href=\"https://t.me/ketronix_dev\">адміністратору бота</a>",
                        parseMode: ParseMode.Html,
                        disableWebPagePreview: true,
                        replyMarkup: inlineKeyboard);
                }
            }
        }
    }
}