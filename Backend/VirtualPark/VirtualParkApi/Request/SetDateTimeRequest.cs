namespace VirtualParkApi.Request;

public class SetDateTimeRequest(DateTime dateTime)
{
    public DateTime DateTime { get; set; } = dateTime.AddHours(3);
}
