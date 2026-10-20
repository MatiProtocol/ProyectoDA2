using NonDomainAbstractions;

namespace Domain;

public class AttractionPunctuation : IPunctuationStrategy
{
    public int CalculatePoints(IPunctuationContext context)
    {
        return context.ActualAttractionType switch
        {
            nameof(TypeAttraction.RollerCoaster) => 100,
            nameof(TypeAttraction.Simulator) => 80,
            nameof(TypeAttraction.Show) => 60,
            nameof(TypeAttraction.InteractiveZone) => 40,
            _ => 0
        };
    }
}
