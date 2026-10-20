using Domain;

namespace Tests.DomainTest;

[TestClass]
public class VisitorParkInformationTest
{
    [TestMethod]
    public void VisitorParkInformation_IsNotNull()
    {
        var result = new VisitorParkInformation();

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void SetId_GetCorrectly()
    {
        var visitorParkInformation = new VisitorParkInformation
        {
            Id = 1
        };

        Assert.AreEqual(1, visitorParkInformation.Id);
    }

    [TestMethod]
    public void SetNfc_GetCorrectly()
    {
        var visitorParkInformation = new VisitorParkInformation();
        Guid id = Guid.Empty;

        visitorParkInformation.NFC = id;

        Assert.AreEqual(Guid.Empty, visitorParkInformation.NFC);
    }

    [TestMethod]
    public void SetPremiumVisitorMembership_GetCorrectly()
    {
        var visitorParkInformation = new VisitorParkInformation()
        {
            VisitorMembership = nameof(Membership.Premium)
        };

        Assert.AreEqual(nameof(Membership.Premium), visitorParkInformation.VisitorMembership);
    }

    [TestMethod]
    public void SetVipMembership_GetCorrectly()
    {
        var visitorParkInformation = new VisitorParkInformation()
        {
            VisitorMembership = nameof(Membership.Vip)
        };

        Assert.AreEqual(nameof(Membership.Vip), visitorParkInformation.VisitorMembership);
    }

    [TestMethod]
    public void SetMembership_GetCorrectly()
    {
        var visitorParkInformation = new VisitorParkInformation
        {
            VisitorMembership = nameof(Membership.Standard)
        };

        Assert.AreEqual(nameof(Membership.Standard), visitorParkInformation.VisitorMembership);
    }

    [TestMethod]
    public void SetPoints_GetCorrectly()
    {
        var visitorParkInformation = new VisitorParkInformation
        {
            Points = 1
        };

        Assert.AreEqual(1, visitorParkInformation.Points);
    }

    [TestMethod]
    public void SetTickets_GetCorrectly()
    {
        var visitorParkInformation = new VisitorParkInformation
        {
            Tickets = []
        };

        Assert.IsNotNull(visitorParkInformation.Tickets);
    }

    [TestMethod]
    public void SetVisitedAttractionList_GetCorrectly()
    {
        var information = new VisitorParkInformation();
        var visitedAttractionList = new List<VisitedAttraction>
        {
            new (1, 1, DateTime.Parse("10/10/2010"))
        };

        information.VisitedAttractions = visitedAttractionList;
        Assert.AreEqual(information.VisitedAttractions.Count, visitedAttractionList.Count);
    }

    [TestMethod]
    public void SetTodayPoints_GetCorrectly()
    {
        var information = new VisitorParkInformation()
        {
            TodayPoints = 1
        };

        Assert.AreEqual(1, information.TodayPoints);
    }

    [TestMethod]
    public void SetRedeemPointsToday_SetCorrectly()
    {
        var information = new VisitorParkInformation()
        {
            RedeemPointsToday = 1
        };

        Assert.AreEqual(1, information.RedeemPointsToday);
    }

    [TestMethod]
    public void SetRewardRedemptions_GetCorrectly()
    {
        var visitorParkInformation = new VisitorParkInformation
        {
            RewardRedemptions = []
        };

        Assert.IsNotNull(visitorParkInformation.RewardRedemptions);
    }

    [TestMethod]
    public void SetPointsHistory_GetCorrectly()
    {
        var visitorParkInformation = new VisitorParkInformation
        {
            PointsHistory = []
        };

        Assert.IsNotNull(visitorParkInformation.PointsHistory);
    }
}
