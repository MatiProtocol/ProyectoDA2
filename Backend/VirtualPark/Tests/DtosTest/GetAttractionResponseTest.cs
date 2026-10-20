using Domain;
using VirtualParkApi.Response;

namespace Tests.DtosTest;

[TestClass]
public class GetAttractionResponseTest
{
    private GetAttractionResponse _response = null!;
    [TestInitialize]
    public void Initialize()
    {
        _response = new GetAttractionResponse(new Attraction());
    }

    [TestMethod]
    public void CreateGetAttractionResponse_NotNull()
    {
        Assert.IsNotNull(_response);
    }

    [TestMethod]
    public void SetName_GetCorrectly()
    {
        _response.Name = "Name";
        Assert.AreEqual(_response.Name, "Name");
    }

    [TestMethod]
    public void SetTypeAttraction_GetCorrectly()
    {
        _response.Type = nameof(TypeAttraction.RollerCoaster);
        Assert.AreEqual(_response.Type, nameof(TypeAttraction.RollerCoaster));
    }

    [TestMethod]
    public void SetMinAge_GetCorrectly()
    {
        _response.MinAge = 22;
        Assert.AreEqual(_response.MinAge, 22);
    }

    [TestMethod]
    public void SetMaxCapacity_GetCorrectly()
    {
        _response.MaxCapacity = 3;
        Assert.AreEqual(_response.MaxCapacity, 3);
    }

    [TestMethod]
    public void SetCurrentCapacity_GetCorrectly()
    {
        _response.CurrentCapacity = 3;
        Assert.AreEqual(_response.CurrentCapacity, 3);
    }

    [TestMethod]
    public void SetDescription_GetCorrectly()
    {
        _response.Description = "Buenas tardes";
        Assert.AreEqual(_response.Description, "Buenas tardes");
    }

    [TestMethod]
    public void UseConstructorWithAttraction_CreateCorrectly()
    {
        var attraction = new Attraction
        {
            Name = "Name",
            TypeAttraction = TypeAttraction.RollerCoaster,
            MinAge = 22,
            MaxCapacity = 3,
            CurrentCapacity = 3,
            Description = "Buenas tardes"
        };

        var responseAttraction = new GetAttractionResponse(attraction);
        Assert.AreEqual(responseAttraction.Name, attraction.Name);
    }

    [TestMethod]
    public void SetId_GetCorrectly()
    {
        _response.Id = 1;
        Assert.AreEqual(_response.Id, 1);
    }
}
