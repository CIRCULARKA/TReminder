using System.IO;

namespace TReminder.Application.Logging
{
    public class FileLogger : ILogger
    {
        private readonly TextWriter _writer;

        public FileLogger(TextWriter wirter) { _writer = wirter; }

        ~FileLogger() { _writer.Dispose(); }

        public void Log(string info)
        {
            _writer.Write(info);
            _writer.Flush();
        }
    }
}
