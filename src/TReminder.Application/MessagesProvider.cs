using System;
using System.IO;
using System.Text.Json;

namespace TReminder.Application
{
    public class MessagesProvider
    {
        private Messages _messages;

        /// <summary>
        /// Gets messages from messages.{langCode}.json from root of the assembly by default
        /// </summary>
        public MessagesProvider(string langCode)
        {
            Messages = JsonSerializer.Deserialize<Messages>(
                File.ReadAllText($"messages.{langCode}.json")
            );
        }

        public MessagesProvider(string langCode, StreamReader reader)
        {
            Messages = JsonSerializer.Deserialize<Messages>(
                reader.ReadToEnd()
            );
        }

        public Messages Messages { get; init; }

        private void ValidateLanguageCode(string langCode)
        {
            var exception = new ArgumentException("Invalid language code");

            if (string.Compare(langCode, "ru", ignoreCase: true) != 0 &&
                string.Compare(langCode, "en", ignoreCase: true) != 0)
                throw exception;
        }
    }
}
