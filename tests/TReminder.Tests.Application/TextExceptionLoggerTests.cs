using System;
using System.IO;
using Xunit;
using TReminder.Application.Logging;

namespace TReminder.Tests.Application
{
    public class TextExceptionLoggerTests
    {
        [Fact]
        public void DoesProduceOutput()
        {
            var targetPath = "text-log.txt";
            using (var writer = new StreamWriter(targetPath))
            {
                // Arrange
                IExceptionLogger logger = new TextExceptionLogger(
                    writer
                );

                var exception = new Exception("Some message");

                // Act
                logger.LogException(exception);
            }

            using (var reader = new StreamReader(targetPath))
            {
                var output = reader.ReadToEnd().Split("\n", StringSplitOptions.RemoveEmptyEntries);

                // Assert
                var minLinesGenerated = 6;
                Assert.True(output.Length >= minLinesGenerated);
            }

        }
    }
}
