using NonDomainAbstractions;

namespace Domain;

public class ComboPunctuation(int comboTimeMinutes = 5) : IPunctuationStrategy
{
    private readonly int _comboTimeMinutes = comboTimeMinutes;
    private const int BASE_POINTS = 50;
    private const int COMBO_BONUS = 50;

    public int CalculatePoints(IPunctuationContext context)
    {
        var points = BASE_POINTS;
        if(context.PreviousAccesses.Count == 0)
        {
            return points;
        }

        var recentAccesses = context.PreviousAccesses
            .Where(a => (context.ActualAttractionAccessTime - a.GetVisitDate()).TotalMinutes <= _comboTimeMinutes)
            .Where(a => a.GetAttractionType() != context.ActualAttractionType)
            .ToList();

        if(recentAccesses.Count != 0)
        {
            points += COMBO_BONUS;
        }

        return points;
    }
}
