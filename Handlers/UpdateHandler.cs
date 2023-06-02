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
            if (message is not null && message.Text is not null)
            {
                if (message.Text.Contains("/chgroup"))
                {
                    string? groupToSwitch = null;
                    if (message.Text.Split().Length > 1)
                    {
                        groupToSwitch = message.Text.Split()[1];
                    }

                    if (groupToSwitch is not null)
                    {
                        bot.SendChatAction(message.Chat.Id, ChatAction.Typing);
                        bot.SendMessage(
                            message.Chat.Id,
                            $"Ви успішно змінили групу на {groupToSwitch}");
                    }
                    else
                    {
                        bot.SendChatAction(message.Chat.Id, ChatAction.Typing);
                        bot.SendMessage(
                            message.Chat.Id,
                            "Ви неправильно використали команду заміни групи. \n \n Правильний синтаксис:"
                            + " \n <code>/chgroup group</code>"
                            + "\n \n Приклад:"
                            + "\n <code>/chgroup КІУКІ-22-7</code>",
                            null,
                            "HTML");
                    }
                }
                else if (message.Text.Contains("/register"))
                {
                    bot.SendChatAction(message.Chat.Id, ChatAction.Typing);
                    bot.SendMessage(
                        message.Chat.Id,
                        "Задля того щоб зареєструвати чат чи змінити групу використовуйте команду /chgroup");
                }
            }
        }
    }
}