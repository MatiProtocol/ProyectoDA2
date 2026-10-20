using VirtualParkApi.Request;

namespace Tests.DtosTest;

[TestClass]
public class GetPointsHistoryRequestTest
{
    private GetPointsHistoryRequest _request = null!;

    [TestInitialize]
    public void Initialize()
    {
        _request = new GetPointsHistoryRequest
        {
            VisitorId = 1,
            StartDate = new DateTime(2025, 1, 1),
            EndDate = new DateTime(2025, 12, 31)
        };
    }

    [TestMethod]
    public void GetPointsHistoryRequest_IsNotNull()
    {
        Assert.IsNotNull(_request);
    }

    [TestMethod]
    public void SetVisitorId_GetCorrectly()
    {
        _request.VisitorId = 5;
        Assert.AreEqual(5, _request.VisitorId);
    }

    [TestMethod]
    public void SetStartDate_GetCorrectly()
    {
        var newDate = new DateTime(2025, 6, 1);
        _request.StartDate = newDate;
        Assert.AreEqual(newDate, _request.StartDate);
    }

    [TestMethod]
    public void SetEndDate_GetCorrectly()
    {
        var newDate = new DateTime(2025, 6, 30);
        _request.EndDate = newDate;
        Assert.AreEqual(newDate, _request.EndDate);
    }

    [TestMethod]
    public void SetStartDateToNull_GetCorrectly()
    {
        _request.StartDate = null;
        Assert.IsNull(_request.StartDate);
    }

    [TestMethod]
    public void SetEndDateToNull_GetCorrectly()
    {
        _request.EndDate = null;
        Assert.IsNull(_request.EndDate);
    }

    [TestMethod]
    public void DefaultVisitorId_GetCorrectly()
    {
        Assert.AreEqual(1, _request.VisitorId);
    }

    [TestMethod]
    public void DefaultStartDate_GetCorrectly()
    {
        Assert.AreEqual(new DateTime(2025, 1, 1), _request.StartDate);
    }

    [TestMethod]
    public void DefaultEndDate_GetCorrectly()
    {
        Assert.AreEqual(new DateTime(2025, 12, 31), _request.EndDate);
    }

    [TestMethod]
    public void SetVisitorIdToZero_GetCorrectly()
    {
        _request.VisitorId = 0;
        Assert.AreEqual(0, _request.VisitorId);
    }

    [TestMethod]
    public void SetVisitorIdToNegative_GetCorrectly()
    {
        _request.VisitorId = -1;
        Assert.AreEqual(-1, _request.VisitorId);
    }

    [TestMethod]
    public void SetVisitorIdToMaxValue_GetCorrectly()
    {
        _request.VisitorId = int.MaxValue;
        Assert.AreEqual(int.MaxValue, _request.VisitorId);
    }

    [TestMethod]
    public void SetStartDateToMinValue_GetCorrectly()
    {
        _request.StartDate = DateTime.MinValue;
        Assert.AreEqual(DateTime.MinValue, _request.StartDate);
    }

    [TestMethod]
    public void SetEndDateToMaxValue_GetCorrectly()
    {
        _request.EndDate = DateTime.MaxValue;
        Assert.AreEqual(DateTime.MaxValue, _request.EndDate);
    }

    [TestMethod]
    public void SetBothDatesToNull_GetCorrectly()
    {
        _request.StartDate = null;
        _request.EndDate = null;
        Assert.IsNull(_request.StartDate);
        Assert.IsNull(_request.EndDate);
    }
}
