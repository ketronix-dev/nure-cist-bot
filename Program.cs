using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.GettingUpdates;
using NureCistBot.Handlers;

namespace NureCistBot
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Start!");
            var bot = new BotClient("6077780376:AAF5_q0dTZYQXc9hpRCoE0TcRov_9OYhlYg");
            var updates = bot.GetUpdates();
            while (true)
            {
                if (updates.Length > 0)
                {
                    foreach (var update in updates)
                    {
                        if (update.Type == UpdateType.Message)
                        {
                            try
                            {
                                var message = update.Message;
                                if (message is not null)
                                {
                                    bot.SendChatAction(message.Chat.Id, ChatAction.UploadDocument);
                                    Thread.Sleep(5000);
                                    bot.SendMessage(message.Chat.Id, "Hello World!");
                                }
                            }
                            catch (System.Exception)
                            {
                                ExceptionHandler.SendToLogs(bot, update);
                            }
                        }
                    }
                    updates = bot.GetUpdates(offset: updates.Max(u => u.UpdateId) + 1);
                }
                else
                {
                    updates = bot.GetUpdates();
                }
            }
        }
    }
}