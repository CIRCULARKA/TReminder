using System;
using System.IO;
using System.Text.Json;

namespace TReminder.Application
{
    public class MessagesProvider
    {
        private Messages _messages;

        public MessagesProvider(string langCode)
        {
            var exception = new ArgumentException("Invalid language code");

            if (string.Compare(langCode, "ru", ignoreCase: true) != 0 &&
                string.Compare(langCode, "en", ignoreCase: true) != 0)
                throw exception;

            Messages = JsonSerializer.Deserialize<Messages>(
                File.ReadAllText($"messages.{langCode}.json")
            );
        }

        public Messages Messages { get; init; }
    }
}
