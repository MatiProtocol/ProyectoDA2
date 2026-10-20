namespace NonDomainAbstractions;

public interface IPunctuationStrategy
{
    public int CalculatePoints(IPunctuationContext context);
}
