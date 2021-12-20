using System;
using System.Linq;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TReminder.Application.Messaging;

namespace TReminder
{
    class Program
    {
        private static string _pathToLogs = "logs";

        private static MessagesProvider _englishMessages;

        private static MessagesProvider _russianMessages;

        static async Task Main(string[] args)
        {
            try
            {
                InitializeMessagesProviders();

                var client = CreateBotClient();
                await ConfigureBotCommandsAsync(client);
                StartBotClient(client);

            }
            catch (Exception e)
            {
                LogException(FormatExceptionInformation(e));
            }

            Console.ReadLine();
        }

        public static async Task HandleUpdate(ITelegramBotClient client, Update update, CancellationToken token)
        {
            if (update.Type != UpdateType.Message)
                return;

            if (update.Message!.Type != MessageType.Text)
                return;

            var incomingChatId = update.Message.Chat.Id;
            var incomingMessage = update.Message.Text;

            var langCode = update.Message.From.LanguageCode;

            var upcomingMessage = update.Message.Text.Length >= 4096 ?
                GetMessage(langCode, "YourMessageIsTooLong") :
                GetMessage(langCode, "YouSentTheMessage");

            await client.SendTextMessageAsync(
                chatId: incomingChatId,
                text: upcomingMessage,
                cancellationToken: token
            );
        }

        public static Task HandleError(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            LogException(FormatExceptionInformation(exception));

            Console.WriteLine($"Something went wrong. Error was logged in root of the application");

            return Task.CompletedTask;
        }

        private static ITelegramBotClient CreateBotClient()
        {
            var api = ConfigurationManager.AppSettings["apiKey"];

            return new TelegramBotClient(api);
        }

        private static void StartBotClient(ITelegramBotClient client)
        {
            client.StartReceiving(
                HandleUpdate,
                HandleError
            );
        }

        private async static Task ConfigureBotCommandsAsync(ITelegramBotClient client)
        {
            var englishCommands = new BotCommand[] {
                new BotCommand { Command = "new", Description = "Create new reminder" },
                new BotCommand { Command = "list", Description = "List all reminders you have" },
            };

            var russianCommands = new BotCommand[] {
                new BotCommand { Command = englishCommands[0].Command, Description = "Создать новое напомнинание" },
                new BotCommand { Command = englishCommands[1].Command, Description = "Список имеющихся напоминаний" }
            };

            await client.SetMyCommandsAsync(englishCommands, languageCode: "en");
            await client.SetMyCommandsAsync(russianCommands, languageCode: "ru");
        }

        private static void LogException(IEnumerable<string> lines)
        {
            System.IO.File.AppendAllLines(
                Path.Combine(_pathToLogs, $"{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}.txt"),
                lines
            );
        }

        private static IEnumerable<string> FormatExceptionInformation(Exception e)
        {
            return new string[] {
                $"[{DateTime.Now.ToString("HH:mm MMMM dd, yyyy")}]",
                $"Message: {e.Message}",
                $"Assembly: {e.Source}",
                $"Method: {e.TargetSite}",
                e.InnerException == null ? "No inner exception" : $"Inner exception message: {e.InnerException.Message}",
                $"Stack trace:\n {e.StackTrace}",
                "\n"
            };
        }

        private static void InitializeMessagesProviders()
        {
            _englishMessages = new MessagesProvider("en");
            _russianMessages = new MessagesProvider("ru");
        }

        private static string GetMessage(string langCode, string messageName)
        {
            try
            {
                var props = Type.GetType(typeof(Messages).AssemblyQualifiedName).GetProperties();
                var targetProperty = Type.GetType(typeof(Messages).AssemblyQualifiedName).
                    GetProperties().Single(p => p.Name == messageName);
                if (langCode == "ru")
                    return targetProperty.GetValue(_russianMessages.Messages) as string;
                else
                    return targetProperty.GetValue(_englishMessages.Messages) as string;
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException("Message name not found");
            }
        }
    }
}
