using System;

namespace TReminder.Application.Bot
{
    public interface IExceptionLogger
    {
        void LogException(Exception e);
    }
}
