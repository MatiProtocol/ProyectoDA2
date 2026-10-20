using Domain;
using NonDomainAbstractions;

namespace Tests.DomainTest;

[TestClass]
public class EventPunctuationTest
{
    private EventPunctuation _strategy = null!;
    private PunctuationContext _context = null!;

    [TestInitialize]
    public void Initialize()
    {
        _strategy = new EventPunctuation(2.0);

        var actualAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.RollerCoaster,
            VisitDate = DateTime.Now
        };

        _context = new PunctuationContext
        {
            VisitorParkInformationId = 1,
            ActualAttraction = actualAttraction,
            ActualAttractionType = nameof(TypeAttraction.RollerCoaster),
            ActualAttractionAccessTime = DateTime.Now,
            ActualAttractionIsSpecial = false
        };
    }

    [TestMethod]
    public void EventPunctuation_ImplementsInterface()
    {
        IPunctuationStrategy strategy = new EventPunctuation();
        Assert.IsNotNull(strategy);
    }

    [TestMethod]
    public void EventPunctuation_WithoutEvent_ReturnsBasePoints()
    {
        _context.ActualAttractionIsSpecial = false;

        var points = _strategy.CalculatePoints(_context);

        Assert.AreEqual(50, points);
    }

    [TestMethod]
    public void EventPunctuation_WithEvent_ReturnsMultipliedPoints()
    {
        _context.ActualAttractionIsSpecial = true;

        var points = _strategy.CalculatePoints(_context);

        Assert.AreEqual(100, points);
    }

    [TestMethod]
    public void EventPunctuation_CustomMultiplier_TriplePoints()
    {
        var tripleStrategy = new EventPunctuation(3.0);
        _context.ActualAttractionIsSpecial = true;

        var points = tripleStrategy.CalculatePoints(_context);

        Assert.AreEqual(150, points);
    }

    [TestMethod]
    public void EventPunctuation_DefaultConstructor_UsesDoubleMultiplier()
    {
        var defaultStrategy = new EventPunctuation();
        _context.ActualAttractionIsSpecial = true;

        var points = defaultStrategy.CalculatePoints(_context);

        Assert.AreEqual(100, points);
    }
}
