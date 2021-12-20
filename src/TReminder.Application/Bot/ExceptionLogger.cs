using System;
using System.IO;

namespace TReminder.Application.Logging
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
                "Message: " + (e.Message == null ? "No information\n" : e.Message + "\n") +
                "Assembly: " + (e.Source == null ? "No information\n" : e.Source + "\n") +
                "Method: " + (e.TargetSite == null ? "No information\n" : e.TargetSite + "\n") +
                (e.InnerException == null ? "No inner exception\n" : $"Inner exception message: {e.InnerException.Message}\n") +
                "Stack trace: " + (e.StackTrace == null ? "No information\n\n" : $"\n{e.StackTrace}" + "\n\n");
        }
    }
}
