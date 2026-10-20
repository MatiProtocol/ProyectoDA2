using Abstraction;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VirtualParkApi.Controllers;
using VirtualParkApi.Request;
using VirtualParkApi.Response;

namespace Tests.ControllersTest;

[TestClass]
public class AttractionsControllerTest
{
    private Mock<IAttractionService> _attractionService = null!;
    private Mock<IIncidenceService> _incidenceService = null!;

    private IAttractionController _attractionsController = null!;
    private IEnumerable<Attraction> _attractions = null!;

    private Attraction _nullAttraction = null!;
    private Attraction _attraction = null!;

    [TestInitialize]
    public void Initialize()
    {
        _attractionService = new Mock<IAttractionService>(MockBehavior.Strict);
        _incidenceService = new Mock<IIncidenceService>(MockBehavior.Strict);

        _attractionsController = new AttractionsController(_attractionService.Object, _incidenceService.Object);

        _attraction = new Attraction
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
        _attractions = new List<Attraction> { _attraction }.AsQueryable();

        _nullAttraction = null!;
    }

    [TestMethod]
    public void CreateAttractionController_IsNotNull()
    {
        Assert.IsNotNull(_attractionsController);
    }

    [TestMethod]
    public void GetAllAttractions_CountTotal()
    {
        _attractionService.Setup(s => s.GetAll()).Returns(_attractions.AsQueryable());

        var response = _attractionsController.GetAll() as OkObjectResult;

        var result = response!.Value as IQueryable<GetAttractionResponse>;

        Assert.AreEqual(1, result!.Count());
    }

    [TestMethod]
    public void GetAttraction_Ok()
    {
        _attractionService.Setup(s => s.GetById(_attraction.Id)).Returns(_attraction);

        var response = _attractionsController.GetById(_attraction.Id) as OkObjectResult;

        var result = response!.Value as GetAttractionResponse;

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void GetAttraction_NotFound()
    {
        _attractionService.Setup(s => s.GetById(99)).Throws(new NullReferenceException());

        Assert.ThrowsException<NullReferenceException>(() => _attractionsController.GetById(99));
    }

    [TestMethod]
    public void RemoveAttraction_Ok()
    {
        _attractionService.Setup(s => s.Delete(It.IsAny<int>()));

        var result = _attractionsController.Remove(_attraction.Id) as OkResult;

        Assert.IsInstanceOfType<OkResult>(result);
    }

    [TestMethod]
    public void RemoveAttraction_WithNonTrackedAttraction_Ok()
    {
        _attractionService.Setup(s => s.Delete(It.IsAny<int>()));

        var result = _attractionsController.Remove(_attraction.Id) as OkResult;

        Assert.IsInstanceOfType<OkResult>(result);
    }

    [TestMethod]
    public void AddAttraction_Ok()
    {
        _attractionService.Setup(s => s.Create(It.IsAny<Attraction>()));

        var result = _attractionsController.Create(new CreateAttractionRequest
        {
            Name = _attraction.Name,
            Description = _attraction.Description,
            Type = _attraction.TypeAttraction.ToString(),
            MinAge = _attraction.MinAge,
            MaxCapacity = _attraction.MaxCapacity,
            CurrentCapacity = _attraction.CurrentCapacity
        }) as OkResult;

        Assert.IsInstanceOfType<OkResult>(result);
    }

    [TestMethod]
    public void AddAttractionWithWrongEmptyName_ThrowsException()
    {
        _attractionService
            .Setup(s => s.Create(It.Is<Attraction>(a => a.Name == string.Empty)))
            .Throws(new ArgumentException());

        _attraction.Name = string.Empty;

        Assert.ThrowsException<ArgumentException>(() =>
            _attractionsController.Create(new CreateAttractionRequest
            {
                Name = _attraction.Name,
                Description = _attraction.Description,
                Type = _attraction.TypeAttraction.ToString(),
                MinAge = _attraction.MinAge,
                MaxCapacity = _attraction.MaxCapacity,
                CurrentCapacity = _attraction.CurrentCapacity
            }));
    }

    [TestMethod]
    public void AddAttractionWithMaxCapacityBelowZero_ThrowsException()
    {
        _attractionService
            .Setup(s => s.Create(It.Is<Attraction>(a => a.MaxCapacity <= 0)))
            .Throws(new ArgumentException());

        _attraction.MaxCapacity = -20;

        Assert.ThrowsException<ArgumentException>(() =>
            _attractionsController.Create(new CreateAttractionRequest
            {
                Name = _attraction.Name,
                Description = _attraction.Description,
                Type = _attraction.TypeAttraction.ToString(),
                MinAge = _attraction.MinAge,
                MaxCapacity = _attraction.MaxCapacity,
                CurrentCapacity = _attraction.CurrentCapacity
            }));
    }

    [TestMethod]
    public void AddAttractionWithMinAgeBelowZero_ThrowsException()
    {
        _attractionService
            .Setup(s => s.Create(It.Is<Attraction>(a => a.MinAge <= 0)))
            .Throws(new ArgumentException());
        _attraction.MinAge = -20;

        Assert.ThrowsException<ArgumentException>(() =>
            _attractionsController.Create(new CreateAttractionRequest
            {
                Name = _attraction.Name,
                Description = _attraction.Description,
                Type = _attraction.TypeAttraction.ToString(),
                MinAge = _attraction.MinAge,
                MaxCapacity = _attraction.MaxCapacity,
                CurrentCapacity = _attraction.CurrentCapacity
            }));
    }

    [TestMethod]
    public void UpdateAttraction_Ok()
    {
        _attractionService.Setup(s => s.Update(It.IsAny<Attraction>()));

        var result = _attractionsController.Update(_attraction.Id, new CreateAttractionRequest
        {
            Name = _attraction.Name,
            Description = _attraction.Description,
            Type = _attraction.TypeAttraction.ToString(),
            MinAge = _attraction.MinAge,
            MaxCapacity = _attraction.MaxCapacity,
            CurrentCapacity = _attraction.CurrentCapacity
        }) as OkResult;

        Assert.IsInstanceOfType<OkResult>(result);
    }

    [TestMethod]
    public void ValidateAttractionAccess_WithVisitorNfc_Ok()
    {
        _attractionService
            .Setup(a
                => a.ValidateAttractionAccess(It.IsAny<int>(), It.IsAny<Guid?>(), It.IsAny<Guid?>()));

        var request = new AttractionAccessRequest
        {
            VisitorNFC = Guid.Empty
        };

        var result = _attractionsController.ValidateAttractionAccess(1, request);

        Assert.IsInstanceOfType<OkResult>(result);
    }

    [TestMethod]
    public void ValidateAttractionAccess_WithQr_Ok()
    {
        _attractionService
            .Setup(a
                => a.ValidateAttractionAccess(It.IsAny<int>(), It.IsAny<Guid?>(), It.IsAny<Guid?>()));

        var request = new AttractionAccessRequest
        {
            TicketQR = Guid.Empty
        };

        var result = _attractionsController.ValidateAttractionAccess(1, request);

        Assert.IsInstanceOfType<OkResult>(result);
    }

    [TestMethod]
    public void AttractionExit_Ok()
    {
        _attractionService.Setup(a => a.DecrementCurrentCapacity(_attraction.Id));

        var result = _attractionsController.AttractionExit(_attraction.Id);

        Assert.IsInstanceOfType<OkResult>(result);
    }

    [TestMethod]
    public void RegisterIncident_Ok()
    {
        _incidenceService.Setup(i => i.Create(It.IsAny<Incidence>()));

        var request = new CreateIncidenceRequestByAttractionController
        {
            Description = string.Empty
        };

        var result = _attractionsController.RegisterIncident(1, request);

        Assert.IsInstanceOfType<OkResult>(result);
    }

    [TestMethod]
    public void DeactivateIncident_Ok()
    {
        _incidenceService.Setup(i => i.DeactivateIncidence(It.IsAny<int>()));

        var result = _attractionsController.DeactivateIncident(1);

        Assert.IsInstanceOfType<OkResult>(result);
    }

    [TestMethod]
    public void GetVisitsBetweenDates_ReturnsCorrectCount()
    {
        var attractionId = 1;
        var startDate = DateTime.Parse("10/10/2001");
        var endDate = DateTime.Parse("10/10/2002");
        var expectedVisitsCount = 5;

        _attractionService
            .Setup(s => s.GetVisitsBetween(attractionId, startDate, endDate))
            .Returns(expectedVisitsCount);

        var response = _attractionsController.GetVisitsBetween(attractionId, startDate, endDate) as OkObjectResult;

        _attractionService.Verify(
            s => s.GetVisitsBetween(attractionId, startDate, endDate),
            Times.Once);

        Assert.AreEqual(200, response!.StatusCode);
    }
}
