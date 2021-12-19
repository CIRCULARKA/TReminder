using System;
using TReminder.Models;
using Xunit;

namespace TReminder.Tests.Models
{
    public class RepetitionSchemaTests
    {
        [Fact]
        public void CanNotBeInvariant()
        {
            // Assert
            var schema = new RepetitionSchema(IntervalType.PerWeek);

            Assert.Equal(IntervalType.PerWeek, schema.IntervalType);

            Assert.Throws<ArgumentException>(
                () => { new RepetitionSchema(IntervalType.PerDay); }
            );

            Assert.Throws<ArgumentException>(
                () => { new RepetitionSchema(IntervalType.PerMonth); }
            );

            Assert.Throws<ArgumentException>(
                () => { new RepetitionSchema(IntervalType.PerWeek); }
            );
        }
    }
}
