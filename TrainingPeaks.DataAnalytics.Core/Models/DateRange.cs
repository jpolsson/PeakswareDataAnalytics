
namespace TrainingPeaks.DataAnalytics.Core.Models
{
    public class DateRange
    {
        public DateTime Start { get; }
        public DateTime End { get; }

        public DateRange(DateTime start, DateTime end)
        {
            if (end < start)
                throw new ArgumentException("End date must be greater than or equal to start date.");

            Start = start;
            End = end;
        }

        // Check if a date falls within the range
        public bool Contains(DateTime date)
        {
            return date >= Start && date <= End;
        }

        public override string ToString() => $"{Start.ToShortDateString()} - {End.ToShortDateString()}";
    }
}
