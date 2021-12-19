using System;

namespace TReminder.Models
{
    public class Reminder
    {
        private RepetitionSchema _repetitionSchema;

        private DateTime _triggerTime;

        private string _notation;

        public Reminder(string name, IntervalType interval, DateTime triggerTime, string notation = null)
        {
            Validate(name, triggerTime);

            _triggerTime = triggerTime;

            _notation = notation;

            var preparedName = name.Trim();
            Name = preparedName;

            _repetitionSchema = new RepetitionSchema(interval);
        }

        public string Name { get; init; }

        public string Notation
        {
            get => _notation == null ? "" : _notation.Trim();
            init => _notation = value;
        }

        public DateTime TriggerTime => _triggerTime;

        public IntervalType IntervalType => _repetitionSchema.IntervalType;

        private void Validate(string name, DateTime triggerTime)
        {
            var wrongNameException = new ArgumentException("Name can't be empty");

            if (name == null)
                throw wrongNameException;
            else if (string.IsNullOrWhiteSpace(name))
                throw wrongNameException;

            if (triggerTime < DateTime.Now)
                throw new ArgumentException("Specified time is invalid");
        }
    }
}
