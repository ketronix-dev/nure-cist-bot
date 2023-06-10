using NureCistBot.Generators;
using NureCistBot.BackendServices;
using NureCistBot.Classes;
using NureCistBot.DateManagment;
using NureCistBot.JsonParsers;
using NureCistBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Net.NetworkInformation;
using Newtonsoft.Json;

namespace NureCistBot.Handlers
{
    public class UpdateHandler
    {
        public static Message? joinMessage;
        public static async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;
            try
            {
                if (update.Type == UpdateType.Message)
                {
                    if (message is not null && message.Text is not null)
                    {
                        if (message.Chat.Type == ChatType.Private && message.Chat.Id == 946530105)
                        {
                            var splited = message.Text.Split('|');
                            if (splited.Length >= 2)
                            {
                                if (message.Text.Contains("/notify"))
                                {
                                    var chats = Database.GetGroups();
                                    foreach (var chat in chats)
                                    {
                                        Console.WriteLine(chat.Id);
                                        await bot.SendChatActionAsync(chat.Id, ChatAction.Typing);
                                        await bot.SendTextMessageAsync(
                                            chat.Id,
                                            "Notify text:" + splited[1]
                                        );
                                    }
                                }
                                if (message.Text.Contains("/ban"))
                                {
                                    Database.BlockGroup(Database.GetGroup(long.Parse(splited[1])));
                                    await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                                    await bot.SendTextMessageAsync(
                                        message.Chat.Id,
                                        "Your are banned:" + splited[1]
                                    );
                                }
                            }
                        }
                        else
                        {
                            if (!Database.IsBlocked(message.Chat.Id))
                            {
                                var ping = new Ping();
                                var source = new Uri("https://cist.nure.ua/");
                                var isAlive = ping.SendPingAsync(source.Host, 500);

                                if (isAlive.Result.Status == IPStatus.Success)
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
                                            var groups = GroupsParser.Parse();

                                            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                                            var group = GroupServices.FindGroupByName(groups, groupToSwitch.ToUpper());

                                            if (group is not null)
                                            {
                                                if (!Database.CheckGroup(message.Chat.Id))
                                                {
                                                    Database.AddGroup(group, message);
                                                    await bot.SendTextMessageAsync(
                                                        message.Chat.Id,
                                                        $"Тепер в цей чат буде відправлятися розклад для групи {groupToSwitch.ToUpper()}");
                                                }
                                                else
                                                {
                                                    Database.UpdateGroup(message, group);
                                                    await bot.SendTextMessageAsync(
                                                        message.Chat.Id,
                                                        $"Ви успішно змінили групу для цього чату, " +
                                                        $"тепер сюди буде відправлятися розклад для {groupToSwitch.ToUpper()}");
                                                }
                                            }
                                            else
                                            {
                                                await bot.SendTextMessageAsync(
                                                    message.Chat.Id,
                                                    "Я не зміг знайти цю групу у базі CIST, можливо вона не була в неї внесена. \n \n" +
                                                    "Будь ласка, завітайте на cist.nure.ua, та перевірте наявність групи. \n \n" +
                                                    "У випадку якщо група там все ж таки присутня, спробуйте скопіювати звідти назву, і використати у боті.\n \n" +
                                                    "<b>УВАГА! Бот буде виводити це повідомлення, якщо ви введете назву групи англійською чи російською мовами.</b>",
                                                    parseMode: ParseMode.Html);
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
                                            "\t <code>/next_week</code> - відправить розклад на наступний тижденью",
                                            parseMode: ParseMode.Html);
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
                                        else
                                        {
                                            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                                            await bot.SendTextMessageAsync(
                                                message.Chat.Id,
                                                "Ви повинні зареєструвати чат перед тим як робити запит. Як це зробити " +
                                                "ви можете дізнатися по команді <code>/help</code>",
                                                parseMode: ParseMode.Html);
                                        }
                                    }
                                    else if (message.Text.Contains("/day"))
                                    {
                                        if (Database.CheckGroup(message.Chat.Id))
                                        {
                                            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                                            var events = ScheduleService.GetCistEvents(
                                                            ScheduleService.GetCistShedule(Database.GetGroup(message.Chat.Id), EnviromentManager.ReadApiKey()),
                                                            DateService.GetToday(),
                                                            DateService.GetToday());
                                            var messageSchedule = MessageGenerator.GenerateMessageForToday(
                                                events,
                                                Database.GetGroup(message.Chat.Id));

                                            if (events.Count is not 0)
                                            {
                                                await bot.SendTextMessageAsync(
                                                    message.Chat.Id,
                                                    messageSchedule,
                                                    parseMode: ParseMode.Html, disableWebPagePreview: true);
                                            }
                                            else
                                            {
                                                await bot.SendTextMessageAsync(
                                                    message.Chat.Id,
                                                    "Пар сьогодні нема, дозволяю відпочити");
                                            }
                                        }
                                        else
                                        {
                                            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                                            await bot.SendTextMessageAsync(
                                                message.Chat.Id,
                                                "Ви повинні зареєструвати чат перед тим як робити запит. Як це зробити " +
                                                "ви можете дізнатися по команді <code>/help</code>",
                                                parseMode: ParseMode.Html);
                                        }
                                    }
                                    else if (message.Text.Contains("/week"))
                                    {
                                        if (Database.CheckGroup(message.Chat.Id))
                                        {
                                            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                                            var weekDates = DateService.GetWeekDates(DateService.GetToday());
                                            var messageSchedule = MessageGenerator.GenerateMessageForWeek(
                                                Database.GetGroup(message.Chat.Id), weekDates[0], weekDates[1], EnviromentManager.ReadApiKey());

                                            await bot.SendTextMessageAsync(
                                                message.Chat.Id,
                                                messageSchedule,
                                                parseMode: ParseMode.Html, disableWebPagePreview: true);
                                        }
                                        else
                                        {
                                            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                                            await bot.SendTextMessageAsync(
                                                message.Chat.Id,
                                                "Ви повинні зареєструвати чат перед тим як робити запит. Як це зробити " +
                                                "ви можете дізнатися по команді <code>/help</code>",
                                                parseMode: ParseMode.Html);
                                        }
                                    }
                                    else if (message.Text.Contains("/next_day"))
                                    {
                                        if (Database.CheckGroup(message.Chat.Id))
                                        {
                                            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                                            var events = ScheduleService.GetCistEvents(
                                                            ScheduleService.GetCistShedule(Database.GetGroup(message.Chat.Id), EnviromentManager.ReadApiKey()),
                                                            DateService.GetNextDay(),
                                                            DateService.GetNextDay());
                                            var messageSchedule = MessageGenerator.GenerateMessageForToday(
                                                events,
                                                Database.GetGroup(message.Chat.Id));

                                            if (events.Count is not 0)
                                            {
                                                await bot.SendTextMessageAsync(
                                                    message.Chat.Id,
                                                    messageSchedule,
                                                    parseMode: ParseMode.Html, disableWebPagePreview: true);
                                            }
                                            else
                                            {
                                                await bot.SendTextMessageAsync(
                                                    message.Chat.Id,
                                                    "Пар в цей день нема, дозволяю відпочити");
                                            }
                                        }
                                        else
                                        {
                                            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                                            await bot.SendTextMessageAsync(
                                                message.Chat.Id,
                                                "Ви повинні зареєструвати чат перед тим як робити запит. Як це зробити " +
                                                "ви можете дізнатися по команді <code>/help</code>",
                                                parseMode: ParseMode.Html);
                                        }
                                    }
                                    else if (message.Text.Contains("/next_week"))
                                    {
                                        if (Database.CheckGroup(message.Chat.Id))
                                        {
                                            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                                            var weekDates = DateService.GetNextWeekDates(DateService.GetToday());
                                            var messageSchedule = MessageGenerator.GenerateMessageForWeek(
                                                Database.GetGroup(message.Chat.Id), weekDates[0], weekDates[1], EnviromentManager.ReadApiKey());

                                            await bot.SendTextMessageAsync(
                                                message.Chat.Id,
                                                messageSchedule,
                                                parseMode: ParseMode.Html, disableWebPagePreview: true);
                                        }
                                        else
                                        {
                                            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                                            await bot.SendTextMessageAsync(
                                                message.Chat.Id,
                                                "Ви повинні зареєструвати чат перед тим як робити запит. Як це зробити " +
                                                "ви можете дізнатися по команді <code>/help</code>",
                                                parseMode: ParseMode.Html);
                                        }
                                    }
                                }
                                else
                                {
                                    await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                                    await bot.SendTextMessageAsync(
                                        message.Chat.Id,
                                        "Нема з'єднання з CIST, якщо адміністратор не помітив це і не повідомив в інфо-каналі - повідомте йому." +
                                        " Детальна інформація була відправлена в спеціальний лог-канал.");
                                }
                            }
                            else
                            {
                                await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                                await bot.SendTextMessageAsync(
                                    message.Chat.Id,
                                    "Цей чат заблокований, за інформацією звертайтеся до адміністратора @ketronix_dev.");
                            }
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
            catch (Exception e)
            {
                await bot.SendTextMessageAsync(
                    -1001638301850,
                    JsonConvert.SerializeObject(update, Formatting.Indented));
                await bot.SendTextMessageAsync(
                    -1001638301850,
                    JsonConvert.SerializeObject(e, Formatting.Indented));
            }
        }
    }
}