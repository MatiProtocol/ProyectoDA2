using Domain;
using NonDomainAbstractions;

namespace Tests.DomainTest;

[TestClass]
public class AttractionPunctuationTest
{
    [TestMethod]
    public void AttractionPunctuation_ImplementsInterface()
    {
        IPunctuationStrategy strategy = new AttractionPunctuation();
        Assert.IsNotNull(strategy);
    }

    [TestMethod]
    public void AttractionPunctuation_CalculatePoints_ReturnsValue()
    {
        var strategy = new AttractionPunctuation();

        var actualAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.RollerCoaster,
            VisitDate = DateTime.Now
        };

        var context = new PunctuationContext
        {
            ActualAttraction = actualAttraction
        };

        var points = strategy.CalculatePoints(context);
        Assert.IsTrue(points >= 0);
    }

    [TestMethod]
    public void PunctuationContext_HasVisitorId()
    {
        var actualAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.RollerCoaster,
            VisitDate = DateTime.Now
        };

        var context = new PunctuationContext
        {
            VisitorParkInformationId = 1,
            ActualAttraction = actualAttraction
        };

        Assert.AreEqual(1, context.VisitorParkInformationId);
    }

    [TestMethod]
    public void PunctuationContext_HasAttractionType()
    {
        var actualAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.RollerCoaster,
            VisitDate = DateTime.Now
        };

        var context = new PunctuationContext
        {
            VisitorParkInformationId = 1,
            ActualAttraction = actualAttraction
        };
        Assert.AreEqual(nameof(TypeAttraction.RollerCoaster), context.ActualAttractionType);
    }

    [TestMethod]
    public void PunctuationContext_HasAccessTime()
    {
        var actualAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.RollerCoaster,
            VisitDate = DateTime.Now
        };

        var context = new PunctuationContext
        {
            VisitorParkInformationId = 1,
            ActualAttraction = actualAttraction
        };
        Assert.IsNotNull(context.ActualAttractionAccessTime);
    }

    [TestMethod]
    public void AttractionPunctuation_RollerCoaster_Returns100Points()
    {
        var strategy = new AttractionPunctuation();
        var actualAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.RollerCoaster,
            VisitDate = DateTime.Now
        };

        var context = new PunctuationContext
        {
            VisitorParkInformationId = 1,
            ActualAttraction = actualAttraction
        };
        var points = strategy.CalculatePoints(context);
        Assert.AreEqual(100, points);
    }

    [TestMethod]
    public void AttractionPunctuation_Simulator_Returns80Points()
    {
        var strategy = new AttractionPunctuation();
        var actualAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.Simulator,
            VisitDate = DateTime.Now
        };

        var context = new PunctuationContext
        {
            VisitorParkInformationId = 1,
            ActualAttraction = actualAttraction
        };
        var points = strategy.CalculatePoints(context);
        Assert.AreEqual(80, points);
    }

    [TestMethod]
    public void AttractionPunctuation_Show_Returns60Points()
    {
        var strategy = new AttractionPunctuation();
        var actualAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.Show,
            VisitDate = DateTime.Now
        };

        var context = new PunctuationContext
        {
            VisitorParkInformationId = 1,
            ActualAttraction = actualAttraction
        };
        var points = strategy.CalculatePoints(context);
        Assert.AreEqual(60, points);
    }

    [TestMethod]
    public void AttractionPunctuation_InteractiveZone_Returns40Points()
    {
        var strategy = new AttractionPunctuation();
        var actualAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.InteractiveZone,
            VisitDate = DateTime.Now
        };

        var context = new PunctuationContext
        {
            VisitorParkInformationId = 1,
            ActualAttraction = actualAttraction
        };
        var points = strategy.CalculatePoints(context);
        Assert.AreEqual(40, points);
    }
}
