namespace TReminder.Models
{
    public class RepetitionSchema
    {
        public RepetitionSchema(IntervalType interval)
        {
            IntervalType = interval;
        }

        public IntervalType IntervalType { get; init; }
    }
}
