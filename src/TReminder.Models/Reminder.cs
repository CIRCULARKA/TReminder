namespace TReminder.Models
{
    public class Reminder
    {
        private RepetitionSchema _repetitionSchema;

        public Reminder(string name, IntervalType interval, int amountPerInterval)
        {
            _repetitionSchema = new RepetitionSchema {
                IntervalType = interval, AmountPerInterval = amountPerInterval
            };
        }

        public string Name { get; init; }

        public string Notation { get; init; }
    }
}
