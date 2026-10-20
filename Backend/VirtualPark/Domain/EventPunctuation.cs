using NonDomainAbstractions;

namespace Domain;

public class EventPunctuation(double eventMultiplier = 2.0) : IPunctuationStrategy
{
    private readonly double _eventMultiplier = eventMultiplier;
    private const int BASE_POINTS = 50;

    public int CalculatePoints(IPunctuationContext context)
    {
        var points = BASE_POINTS;

        if(context.ActualAttractionIsSpecial)
        {
            points = (int)(points * _eventMultiplier);
        }

        return points;
    }
}
