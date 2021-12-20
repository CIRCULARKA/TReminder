using System.Collections.Generic;
using Telegram.Bot.Types;

namespace TReminder.Application.Bot
{
    public class CommandsProvider : ICommandsProvider
    {
        private readonly List<BotCommand> _commands;

        public CommandsProvider()
        {
            _commands = new List<BotCommand>();
        }

        public IEnumerable<BotCommand> GetCommands() =>
            _commands;

        private void CreateCommands()
        {
            _commands.Add(new BotCommand { Command = "new", Description = "Create new reminder" });
            _commands.Add(new BotCommand { Command = "new", Description = "Create new reminder" });

            _commands.Add(new BotCommand { Command = _commands[0].Command, Description = "Создать новое напомнинание" });
            _commands.Add(new BotCommand { Command = _commands[1].Command, Description = "Список имеющихся напоминаний" });

        }
    }
}
