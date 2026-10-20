using Domain;
using VirtualParkApi.Request;

namespace Tests.DtosTest;
[TestClass]
public class CreateAttractionRequestTest
{
    private CreateAttractionRequest _request = null!;

    [TestInitialize]
    public void Initialize()
    {
        var attraction = new Attraction
        {
            Id = 1,
            Name = "Gusanito Loco",
            Description = "Juego para jovenes",
            CurrentCapacity = 10,
            Incidences = [],
            MaxCapacity = 15,
            MinAge = 5,
            TypeAttraction = TypeAttraction.RollerCoaster,
        };
        _request = new CreateAttractionRequest
        {
            Name = attraction.Name,
            Description = attraction.Description,
            Type = attraction.TypeAttraction.ToString(),
            MinAge = attraction.MinAge,
            MaxCapacity = attraction.MaxCapacity,
            CurrentCapacity = attraction.CurrentCapacity
        };
    }

    [TestMethod]
    public void CreateAttractionRequest_IsNotNull()
    {
        Assert.IsNotNull(_request);
    }

    [TestMethod]
    public void SetName_GetCorrectly()
    {
        _request.Name = "Leirbag";
        Assert.AreEqual(_request.Name, "Leirbag");
    }

    [TestMethod]
    public void SetDescription_GetCorrectly()
    {
        _request.Description = "La descripcion salio del chat";
        Assert.AreEqual(_request.Description, "La descripcion salio del chat");
    }

    [TestMethod]
    public void SetTypeAttraction_GetCorrectly()
    {
        _request.Type = nameof(TypeAttraction.RollerCoaster);
        Assert.AreEqual(_request.Type, nameof(TypeAttraction.RollerCoaster));
    }

    [TestMethod]
    public void SetMinAge_GetCorrectly()
    {
        _request.MinAge = 21;
        Assert.AreEqual(_request.MinAge, 21);
    }

    [TestMethod]
    public void SetMaxCapacity_GetCorrectly()
    {
        _request.MaxCapacity = 21;
        Assert.AreEqual(_request.MaxCapacity, 21);
    }

    [TestMethod]
    public void SetCurrentCapacity_GetCorrectly()
    {
        _request.CurrentCapacity = 21;
        Assert.AreEqual(_request.CurrentCapacity, 21);
    }

    [TestMethod]
    public void AttractionToEntity_DoCorrectly()
    {
        _request.Type = nameof(TypeAttraction.RollerCoaster);
        var attraction = _request.ToEntity();
        Assert.IsInstanceOfType<Attraction>(attraction);
    }

    [TestMethod]
    public void SetType_ThrowInvalidOperationException()
    {
        _request.Type = "Roller";
        Assert.ThrowsException<InvalidOperationException>(() => _request.ToEntity());
    }
}
