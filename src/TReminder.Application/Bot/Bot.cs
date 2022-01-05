using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
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

        private readonly ILogger _logger;

        private long? _currentChatId;

        private string _currentMessage;

        public Bot(
            ITelegramBotClient client,
            ICommandsProvider commandsProvider,
            IMessagesProvider messagesProvider,
            ILogger logger,
            IExceptionLogger exceptionLogger)
        {
            _client = client;
            _commandsProvider = commandsProvider;
            _exceptionLogger = exceptionLogger;
            _messagesProvider = messagesProvider;
            _logger = logger;
        }

        public void Start()
        {
            _client.StartReceiving(
                HandleCommand, HandleException
            );
        }

        private async Task HandleCommand(ITelegramBotClient client, Update update, CancellationToken ct)
        {
            _currentChatId = update.Message?.Chat?.Id;
            _currentMessage = update.Message?.Text;

            var langCode = update.Message?.From?.LanguageCode;
            _messagesProvider.ChangeLanguage(langCode);

            try
            {
                if (update.Type != UpdateType.Message)
                    return;

                if (_currentMessage != null)
                    if (_currentMessage.Length >= _maxMessageSize)
                    return;

                _logger.Log(
                    $"[{DateTime.Now.ToShortDateString()}]\n" +
                    "Message: " + (update.Message == null ? "Empty message" : $"\"{update.Message.Text}\"") + "\n" +
                    "From: " + (update.Message.From.FirstName) + "\n\n"
                );

                var upcomingMessage = _messagesProvider[nameof(Messages.ChooseAnAction)];

                var replyMarkup = new InlineKeyboardMarkup(
                    new List<InlineKeyboardButton> {
                        InlineKeyboardButton.WithCallbackData(_messagesProvider[nameof(Messages.NewReminder)], "testcallback"),
                        InlineKeyboardButton.WithCallbackData(_messagesProvider[nameof(Messages.EditReminder)], "testcallback1"),
                    }
                );

                await client.SendTextMessageAsync(
                    chatId: _currentChatId,
                    text: upcomingMessage,
                    replyMarkup: replyMarkup,
                    cancellationToken: ct
                );
            }
            catch (Exception e)
            {
                _exceptionLogger.LogException(e);

                await client.SendTextMessageAsync(
                    chatId: _currentChatId,
                    text:  _messagesProvider[nameof(Messages.SomethingWentWrong)]
                );
            }
        }

        private Task HandleException(ITelegramBotClient client, Exception e, CancellationToken ct)
        {
            _exceptionLogger.LogException(e);

            return Task.CompletedTask;
        }
    }
}
