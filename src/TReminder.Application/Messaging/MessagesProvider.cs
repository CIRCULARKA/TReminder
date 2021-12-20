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

        public string this[string messageName]
        {
            get => GetMessage(messageName);
        }

        public void ChangeLanguage(string languageCode)
        {
            if (languageCode == null)
                throw new ArgumentException("Language code can't be null");

            foreach (var code in _supportedLanguageCodes)
                if (string.Compare(languageCode, code, ignoreCase: true) == 0)
                {
                    _currentLanguageCode = code;
                    return;
                }

            // If there is no matching lang code then just switch it to "en"
            _currentLanguageCode = "en";
        }

        public string GetMessage(string messageName)
        {
            try
            {
                var targetProperty = Type.GetType(typeof(Messages).AssemblyQualifiedName).
                    GetProperties().Single(p => p.Name == messageName);

                return targetProperty.GetValue(_messages[_currentLanguageCode]) as string;
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
    }
}
