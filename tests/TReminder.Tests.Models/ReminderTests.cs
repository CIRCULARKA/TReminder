using System;
using TReminder.Models;
using Xunit;

namespace TReminder.Tests.Models
{
    public class ReminderTests
    {
        [Fact]
        public void CanNotBeInvariant()
        {
            Assert.Throws<ArgumentException>(
                () => { new Reminder("", IntervalType.PerDay); }
            );

            Assert.Throws<ArgumentException>(
                () => { new Reminder("     ", IntervalType.PerDay); }
            );

            Assert.Throws<ArgumentException>(
                () => { new Reminder(null, IntervalType.PerDay); }
            );
        }
    }
}
