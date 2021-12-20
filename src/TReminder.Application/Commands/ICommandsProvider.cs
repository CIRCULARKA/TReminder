using System.Collections.Generic;
using Telegram.Bot.Types;

namespace TReminder.Application.Commands
{
    public interface ICommandsProvider
    {
        IEnumerable<BotCommand> GetCommands();
    }
}
