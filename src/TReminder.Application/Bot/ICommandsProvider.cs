using System.Collections.Generic;
using Telegram.Bot.Types;

namespace TReminder.Application.Bot
{
    public interface ICommandsProvider
    {
        IEnumerable<BotCommand> GetCommands();
    }
}
