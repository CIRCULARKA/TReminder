using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TReminder
{
    class Program
    {
        private static string _pathToLogs = "logs";

        static async Task Main(string[] args)
        {
            try
            {
                throw new Exception("sf");

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

            var upcomingMessage = update.Message.Text.Length >= 4096 ? "Your message is too long" :
                $"You sent me the \"{incomingMessage}\" message";

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
            var apiKey = JsonSerializer.Deserialize<AppConfiguration>(
                System.IO.File.ReadAllText("config.json"),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ).ApiKey;

            return new TelegramBotClient(apiKey);
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
    }
}
