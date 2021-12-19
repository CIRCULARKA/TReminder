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
            var schema = new RepetitionSchema(IntervalType.PerWeek, 3);

            Assert.Equal(IntervalType.PerWeek, schema.IntervalType);
            Assert.Equal(3, schema.AmountPerInterval);

            Assert.Throws<ArgumentException>(
                () => { new RepetitionSchema(IntervalType.PerDay, 0); }
            );

            Assert.Throws<ArgumentException>(
                () => { new RepetitionSchema(IntervalType.PerMonth, -1); }
            );

            Assert.Throws<ArgumentException>(
                () => { new RepetitionSchema(IntervalType.PerWeek, int.MinValue); }
            );
        }
    }
}
