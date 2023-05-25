using Telegram.BotAPI;
using Telegram.BotAPI.GettingUpdates;
using NureCistBot.Handlers;
using NureCistBot.BackendServices;

namespace NureCistBot
{
    class Program
    {
        private static BotClient? bot;
        static void Main()
        {
            Console.WriteLine("Start!");
            if (File.Exists("config-bot.toml"))
            {
                bot = new BotClient(EnviromentManager.ReadBotToken());
            }
            else
            {
                EnviromentManager.Setup();
                bot = new BotClient(EnviromentManager.ReadBotToken());
            }
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
                                UpdateHandler.HandleUpdate(bot, update);
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