using NureCistBot.BackendServices;
using NureCistBot.Classes;
using NureCistBot.JsonParsers;
using NureCistBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace NureCistBot.Handlers
{
    public class UpdateHandler
    {
        public static Message? joinMessage;
        public static async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;
            if (update.Type == UpdateType.Message)
            {
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
                            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                            var group = new Group
                            {
                                Id = 123456,
                                Name = groupToSwitch
                            };

                            if (!Database.CheckGroup(message.Chat.Id))
                            {
                                Database.AddGroup(group, message);
                                await bot.SendTextMessageAsync(
                                    message.Chat.Id,
                                    $"Тепер в цей чат буде відправлятися розклад для групи {groupToSwitch}");
                            }
                            else
                            {
                                Database.UpdateGroup(message, group);
                                await bot.SendTextMessageAsync(
                                    message.Chat.Id,
                                    $"Ви успішно змінили групу для цього чату, " +
                                    "тепер сюди буде відправлятися розклад для {groupToSwitch}");
                            }
                        }
                        else
                        {
                            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                            await bot.SendTextMessageAsync(
                                message.Chat.Id,
                                "Ви неправильно використали команду заміни групи. \n \n Правильний синтаксис:"
                                + " \n <code>/chgroup group</code>"
                                + "\n \n Приклад:"
                                + "\n <code>/chgroup КІУКІ-22-7</code>",
                                null,
                                ParseMode.Html);
                        }
                    }
                    else if (message.Text.Contains("/help"))
                    {
                        await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                        await bot.SendTextMessageAsync(
                            message.Chat.Id,
                            "Задля того щоб зареєструвати чат чи змінити групу використовуйте команду /chgroup");
                    }
                    else if (message.Text.Contains("/info"))
                    {
                        if (Database.CheckGroup(message.Chat.Id))
                        {
                            var fromDb = Database.GetGroup(message.Chat.Id);
                            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                            await bot.SendTextMessageAsync(
                                message.Chat.Id,
                                "Ось витяг з бази даних стосовно цього чату: \n \n" +
                                $"ID чату: {fromDb.Id}\n" +
                                $"Приватний: {fromDb.IsPrivate}\n" +
                                $"Ідентифікатор CIST: {fromDb.CistId}\n" +
                                $"Назва групи у CIST: {fromDb.CistName}\n" +
                                $"Назва чату: {fromDb.ChatName}\n");
                        }
                    }
                    else if (message.Text.Contains("/test_parser"))
                    {
                        var groups = GroupsParser.Parse();
                        bot.SendTextMessageAsync(message.Chat.Id, $"Успішно імпортовано {groups.Count} групп");
                    }
                }
                else if (message is not null && message.NewChatMembers is not null)
                {
                    await NewMemberHandler.HandleNewMember(bot, message.NewChatMembers, message.Chat.Id);
                }
            }
            else if (update.Type == UpdateType.CallbackQuery && update is not null && joinMessage is not null)
            {
                await CallbackHandler.HandleCallbackAsync(bot, update.CallbackQuery, joinMessage.Chat.Id);
            }


        }
    }
}