using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TReminder.Application.Messaging;
using TReminder.Application.Logging;
using TReminder.Application.Commands;

namespace TReminder.Application.Bot
{
    public class Bot
    {
        private const int _maxMessageSize = 4096;

        private readonly ITelegramBotClient _client;

        private readonly ICommandsProvider _commandsProvider;

        private readonly IExceptionLogger _exceptionLogger;

        private readonly IMessagesProvider _messagesProvider;

        public Bot(
            ITelegramBotClient client,
            ICommandsProvider commandsProvider,
            IExceptionLogger exceptionLogger,
            IMessagesProvider messagesProvider)
        {
            _client = client;
            _commandsProvider = commandsProvider;
            _exceptionLogger = exceptionLogger;
            _messagesProvider = messagesProvider;
        }

        public void Start()
        {
            _client.StartReceiving(
                HandleCommand, HandleException
            );
        }

        private async Task HandleCommand(ITelegramBotClient client, Update update, CancellationToken ct)
        {
            throw new Exception("exc");

            if (update.Type != UpdateType.Message)
                return;

            if (update.Message!.Type != MessageType.Text)
                return;

            var incomingChatId = update.Message.Chat.Id;
            var incomingMessage = update.Message.Text;

            var langCode = update.Message.From.LanguageCode;

            if (update.Message.Text.Length >= _maxMessageSize)
                return;

            var upcomingMessage = _messagesProvider.GetMessage(
                update.Message.From.LanguageCode, $"{nameof(Messages.YouSentTheMessage)}"
            ) + $": \"{update.Message.Text}\"";

            await client.SendTextMessageAsync(
                chatId: incomingChatId,
                text: upcomingMessage,
                cancellationToken: ct
            );
        }

        private Task HandleException(ITelegramBotClient client, Exception e, CancellationToken ct)
        {
            _exceptionLogger.LogException(e);

            return Task.CompletedTask;
        }
    }
}
