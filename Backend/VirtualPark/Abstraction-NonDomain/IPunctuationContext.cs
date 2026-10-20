namespace NonDomainAbstractions;

public interface IPunctuationContext
{
    public List<IContextVisitedAttraction> PreviousAccesses { get; set; }
    public DateTime ActualAttractionAccessTime { get; set; }
    public bool ActualAttractionIsSpecial { get; set; }
    public string ActualAttractionType { get; set; }
}
