using System;
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
        private static readonly string _pathToLogsFile = "logs";

        static async Task Main(string[] args)
        {
            var client = CreateBotClient();
            await ConfigureBotCommandsAsync(client);
            StartBotClient(client);

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
            System.IO.File.AppendAllLines(
                $"logs/{DateTime.Now}.txt",
                new string[] {
                    $"[{DateTime.Now.ToString("HH:mm MMMM dd, yyyy")}]",
                    $"Message: {exception.Message}",
                    $"Assembly: {exception.Source}",
                    $"Method: {exception.TargetSite}",
                    exception.InnerException == null ? "No inner exception" : $"Inner exception message: {exception.InnerException.Message}",
                    $"Stack trace:\n {exception.StackTrace}",
                    "\n"
                }
            );

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
    }
}
