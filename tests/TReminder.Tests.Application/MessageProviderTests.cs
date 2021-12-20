using System;
using System.Text.Json;
using System.IO;
using TReminder.Application;
using Xunit;

namespace TReminder.Tests.Application
{
    public class MessageProviderTests
    {
        [Fact]
        public void DoesReturnInCurrentLanguage()
        {
            // Arrange
            var russianProvider = new MessagesProvider("ru", new StreamReader("test-messages.ru.json"));
            var englishProvider = new MessagesProvider("en", new StreamReader("test-messages.en.json"));

            var russianExpected = JsonSerializer.Deserialize<Messages>(
                File.ReadAllText("test-messages.ru.json")
            ).YouSentTheMessage;

            var englishExpected = JsonSerializer.Deserialize<Messages>(
                File.ReadAllText("test-messages.en.json")
            ).YouSentTheMessage;

            // Act
            var russianActual = russianProvider.Messages.YouSentTheMessage;
            var englishActual = englishProvider.Messages.YouSentTheMessage;

            // Assert
            Assert.Equal(russianExpected, russianActual);
            Assert.Equal(englishExpected, englishActual);
        }
    }
}
