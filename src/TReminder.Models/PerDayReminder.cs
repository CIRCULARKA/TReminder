namespace TReminder.Models
{
    public class PerDayReminder : Reminder
    {
        public PerDayReminder(string name) : base(name, IntervalType.PerDay)
        {

        }
    }
}
