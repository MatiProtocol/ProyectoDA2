using Domain;
using NonDomainAbstractions;

namespace Tests.DomainTest;

[TestClass]
public class ComboPunctuationTest
{
    private ComboPunctuation _strategy = null!;
    private PunctuationContext _context = null!;
    private DateTime _now;

    [TestInitialize]
    public void Initialize()
    {
        _strategy = new ComboPunctuation(5);

        var actualAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.RollerCoaster,
            VisitDate = DateTime.Now
        };

        _now = DateTime.Now;
        _context = new PunctuationContext
        {
            VisitorParkInformationId = 1,
            ActualAttraction = actualAttraction,
            ActualAttractionType = nameof(TypeAttraction.RollerCoaster),
            ActualAttractionAccessTime = _now,
            PreviousAccesses = []
        };
    }

    [TestMethod]
    public void ComboPunctuation_ImplementsInterface()
    {
        IPunctuationStrategy strategy = new ComboPunctuation();
        Assert.IsNotNull(strategy);
    }

    [TestMethod]
    public void PunctuationContext_HasPreviousAccesses()
    {
        Assert.IsNotNull(_context.PreviousAccesses);
    }

    [TestMethod]
    public void ComboPunctuation_WithNoCombo_ReturnsBasePoints()
    {
        var points = _strategy.CalculatePoints(_context);
        Assert.AreEqual(50, points);
    }

    [TestMethod]
    public void ComboPunctuation_WithCombo_ReturnsBonusPoints()
    {
        _context.PreviousAccesses.Add(new VisitedAttraction
        {
            AttractionType = TypeAttraction.Show,
            VisitDate = _now.AddMinutes(-3)
        });

        var points = _strategy.CalculatePoints(_context);

        Assert.AreEqual(100, points);
    }

    [TestMethod]
    public void ComboPunctuation_SameAttractionType_NoBonus()
    {
        _context.PreviousAccesses.Add(new VisitedAttraction
        {
            AttractionType = TypeAttraction.RollerCoaster,
            VisitDate = _now.AddMinutes(-3)
        });

        var points = _strategy.CalculatePoints(_context);

        Assert.AreEqual(50, points);
    }

    [TestMethod]
    public void ComboPunctuation_OutsideTimeWindow_NoBonus()
    {
        _context.PreviousAccesses.Add(new VisitedAttraction
        {
            AttractionType = TypeAttraction.Show,
            VisitDate = _now.AddMinutes(-10)
        });

        var points = _strategy.CalculatePoints(_context);

        Assert.AreEqual(50, points);
    }
}
