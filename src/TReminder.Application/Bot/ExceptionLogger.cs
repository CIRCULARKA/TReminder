using System;
using System.IO;

namespace TReminder.Application.Bot
{
    public class TextExceptionLogger : IExceptionLogger
    {
        private TextWriter _writer;

        public TextExceptionLogger(TextWriter writer)
        {
            _writer = writer;
        }

        public void LogException(Exception e)
        {
            _writer.Write(
                FormatExceptionInformation(e)
            );
        }

        private string FormatExceptionInformation(Exception e)
        {
            return $"[{DateTime.Now.ToString("HH:mm MMMM dd, yyyy")}]\n" +
                $"Message: {e.Message}\n" +
                $"Assembly: {e.Source}\n" +
                $"Method: {e.TargetSite}\n" +
                e.InnerException == null ? "No inner exception" : $"Inner exception message: {e.InnerException.Message}\n" +
                $"Stack trace:\n {e.StackTrace}\n\n";
        }
    }
}
