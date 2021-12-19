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
                () => { new Reminder("", IntervalType.PerDay, 0); }
            );

            Assert.Throws<ArgumentException>(
                () => { new Reminder("     ", IntervalType.PerDay, 0); }
            );

            Assert.Throws<ArgumentException>(
                () => { new Reminder(null, IntervalType.PerDay, 0); }
            );

            Assert.Throws<ArgumentException>(
                () => { new Reminder("Workout", IntervalType.PerDay, -1); }
            );
        }
    }
}
