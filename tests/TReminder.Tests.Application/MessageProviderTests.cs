using System;
using System.Text.Json;
using System.IO;
using Xunit;
using TReminder.Application.Messaging;

namespace TReminder.Tests.Application
{
    public class MessageProviderTests
    {
        [Fact]
        public void DoesReturnInCurrentLanguage()
        {
            // Arrange
            var provider = new MessagesProvider();

            var russianExpected = JsonSerializer.Deserialize<Messages>(
                File.ReadAllText("test-messages.ru.json")
            ).YouSentTheMessage;

            var englishExpected = JsonSerializer.Deserialize<Messages>(
                File.ReadAllText("test-messages.en.json")
            ).YouSentTheMessage;

            // Act
            provider.ChangeLanguage("ru");
            var russianActual = provider[nameof(Messages.YouSentTheMessage)];
            provider.ChangeLanguage("en");
            var englishActual = provider[nameof(Messages.YouSentTheMessage)];

            // Assert
            Assert.Equal(russianExpected, russianActual);
            Assert.Equal(englishExpected, englishActual);
        }
    }
}
