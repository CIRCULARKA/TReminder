using System;

namespace TReminder.Models
{
    public class PerDayReminder : Reminder
    {
        public PerDayReminder(string name, string notation, DateTime triggerTime) :
            base(name, IntervalType.PerDay, triggerTime, notation) { }
    }
}
