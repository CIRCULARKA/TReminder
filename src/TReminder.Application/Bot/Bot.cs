using System;
using System.Linq;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TReminder.Application.Messaging;

namespace TReminder.Application.Bot
{
    public class Bot
    {
        private readonly ITelegramBotClient _client;

        private readonly ICommandsProvider _commandsProvider;

        public Bot(ITelegramBotClient client, ICommandsProvider commandsProvider)
        {
            _client = client;
            _commandsProvider = commandsProvider;
        }

        public void Start()
        {
            _client.StartReceiving(
                HandleCommand, HandleException
            );
        }

        private async Task HandleCommand(ITelegramBotClient client, Update update, CancellationToken ct)
        {

        }

        private Task HandleException(ITelegramBotClient client, Exception e, CancellationToken ct)
        {
            return Task.CompletedTask;
        }
    }
}
