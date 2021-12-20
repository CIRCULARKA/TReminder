using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;

namespace TReminder.Application.Messaging
{
    public class MessagesProvider : IMessagesProvider
    {
        private readonly Dictionary<string, Messages> _messages;

        private readonly string[] _supportedLanguageCodes = new string[] {
            "ru",
            "en"
        };

        private string _currentLanguageCode;

        /// <summary>
        /// Gets messages from messages.{langCode}.json from root of the assembly by default
        /// </summary>
        public MessagesProvider()
        {
            _messages = LoadMessagesForLanguages();
        }

        public string GetMessage(string langCode, string messageName)
        {
            _currentLanguageCode = langCode;

            ValidateLanguageCode();

            try
            {
                var targetProperty = Type.GetType(typeof(Messages).AssemblyQualifiedName).
                    GetProperties().Single(p => p.Name == messageName);

                return targetProperty.GetValue(_messages[langCode]) as string;
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException("Message name not found");
            }
        }

        private Dictionary<string, Messages> LoadMessagesForLanguages()
        {
            var result = new Dictionary<string, Messages>();

            foreach (var code in _supportedLanguageCodes)
            {
                try
                {
                    var json = File.ReadAllText($"messages.{code}.json");
                    var messages = JsonSerializer.Deserialize<Messages>(
                        json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                    result.Add(code, messages);
                }
                catch (Exception)
                {
                    throw new Exception($"Can't find messages.{code}.json to load messages");
                }
            }

            return result;
        }

        private void ValidateLanguageCode()
        {
            var exception = new ArgumentException(
                $"Unsupported language code. (\"{_currentLanguageCode}\" passed)"
            );

            if (_currentLanguageCode == null)
                throw exception;

            foreach (var code in _supportedLanguageCodes)
                if (string.Compare(_currentLanguageCode, code, ignoreCase: true) == 0)
                    return;

            throw exception;
        }
    }
}
