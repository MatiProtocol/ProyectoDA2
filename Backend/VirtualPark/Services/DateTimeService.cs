using NonDomainAbstractions;

namespace Services;

public class DateTimeService : IDateTimeService
{
    private static DateTime? _now = null;

    public DateTime Now
    {
        get
        {
            var now = _now ?? DateTime.Now;
            return now.AddHours(-3);
        }

        set
        {
            _now = value;
        }
    }
}
