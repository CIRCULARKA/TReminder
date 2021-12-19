using System;

namespace TReminder.Models
{
    public class Reminder
    {
        private RepetitionSchema _repetitionSchema;

        public Reminder(string name, IntervalType interval, int amountPerInterval)
        {
            var wrongNameException = new ArgumentException("Name can't be empty");

            if (name == null)
                throw wrongNameException;
            else if (string.IsNullOrWhiteSpace(name))
                throw wrongNameException;

            var preparedName = name.Trim();

            Name = preparedName;

            _repetitionSchema = new RepetitionSchema(interval, amountPerInterval);
        }

        public string Name { get; init; }

        public string Notation { get; init; }
    }
}
