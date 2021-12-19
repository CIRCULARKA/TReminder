using System;

namespace TReminder.Models
{
    public class RepetitionSchema
    {
        public RepetitionSchema(IntervalType interval, int amountPerInterval)
        {
            IntervalType = interval;

            if (amountPerInterval <= 0)
                throw new ArgumentException(
                    $"Amount per interval must be more than zero"
                );
        }

        public IntervalType IntervalType { get; init; }

        public int AmountPerInterval { get; init; }
    }
}
