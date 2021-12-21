using System;
using System.IO;
using System.Text;
using System.Configuration;
using Telegram.Bot;
using TReminder.Application.Bot;
using TReminder.Application.Messaging;
using TReminder.Application.Logging;
using TReminder.Application.Commands;

namespace TReminder
{
    class Program
    {
        static void Main(string[] args)
        {
            var apiKey = ConfigurationManager.AppSettings["apiKey"];
            var client = new TelegramBotClient(apiKey);

            CreateBot(client).Start();

            Console.ReadLine();
        }

        static Bot CreateBot(ITelegramBotClient client)
        {
            var now = DateTime.Now;

            var logsDirectory = "logs";

            var errorsFolder = Path.Combine(logsDirectory, "errors");
            var eventsFolder = Path.Combine(logsDirectory, "events");

            Directory.CreateDirectory(errorsFolder);
            Directory.CreateDirectory(eventsFolder);

            return new Bot(
                client,
                new CommandsProvider(),
                new MessagesProvider(),
                new FileLogger(
                    new StreamWriter(
                        Path.Combine(eventsFolder, $"{now.Day}-{now.Month}-{now.Year}.txt"),
                        append: true,
                        encoding: Encoding.UTF8
                    )
                ),
                new TextExceptionLogger(
                    new StreamWriter(
                        Path.Combine(errorsFolder, $"{now.Day}-{now.Month}-{now.Year}.txt"),
                        append: true,
                        encoding: Encoding.UTF8
                    )
                )
            );
        }
    }
}
