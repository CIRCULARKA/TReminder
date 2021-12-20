using System;

namespace TReminder.Application.Logging
{
    public interface IExceptionLogger
    {
        void LogException(Exception e);
    }
}
