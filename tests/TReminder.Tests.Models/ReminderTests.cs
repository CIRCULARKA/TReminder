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
            var triggerTime = DateTime.Now + new TimeSpan(hours: 1, 0, 0);
            var reminder = new Reminder("Workout", IntervalType.PerDay, triggerTime);

            Assert.Equal("Workout", reminder.Name);
            Assert.Equal(triggerTime.ToShortDateString(), reminder.TriggerTime.ToShortDateString());
            Assert.Equal(IntervalType.PerDay, reminder.IntervalType);

            Assert.Throws<ArgumentException>(
                () => { new Reminder("", IntervalType.PerDay, DateTime.Now); }
            );

            Assert.Throws<ArgumentException>(
                () => { new Reminder("     ", IntervalType.PerDay, DateTime.Now); }
            );

            Assert.Throws<ArgumentException>(
                () => { new Reminder(null, IntervalType.PerDay, DateTime.Now); }
            );
        }
    }
}
