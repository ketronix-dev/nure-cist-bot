using NureCistBot.BackendServices;
using NureCistBot.Handlers;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;


TelegramBotClient botClient;

if (File.Exists("config-bot.toml"))
{
    botClient = new TelegramBotClient(EnviromentManager.ReadBotToken());
}
else
{
    EnviromentManager.Setup();
    botClient = new TelegramBotClient(EnviromentManager.ReadBotToken());
}

using CancellationTokenSource cts = new();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = new UpdateType[]
    {
        UpdateType.Message,
        UpdateType.ChatMember,
        UpdateType.CallbackQuery
    }
};

botClient.StartReceiving(
    updateHandler: UpdateHandler.HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var errorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };
    
    return Task.CompletedTask;
}