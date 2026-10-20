using Abstraction;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VirtualParkApi.Controllers;
using VirtualParkApi.Request;
using VirtualParkApi.Response;

namespace Tests.ControllersTest;

[TestClass]
public class IncidencesControllerTest
{
    private Mock<IIncidenceService> _mockService = null!;
    private IIncidenceController _controller = null!;
    private Incidence _incidence = null!;
    private CreateIncidenceRequest _createRequest = null!;

    [TestInitialize]
    public void Initialize()
    {
        _mockService = new Mock<IIncidenceService>(MockBehavior.Strict);
        _controller = new IncidencesController(_mockService.Object);
        _incidence = new Incidence
        {
            Id = 1,
            Description = "Test incidence",
            AttractionId = 1,
            Active = true
        };

        _createRequest = new CreateIncidenceRequest
        {
            Description = "Test incidence",
            AttractionId = 1
        };
    }

    [TestCleanup]
    public void Cleanup()
    {
        _mockService.VerifyAll();
    }

    [TestMethod]
    public void Constructor_WithValidService_CreatesController()
    {
        Assert.IsNotNull(_controller);
    }

    [TestMethod]
    public void Create_WithValidRequest_ReturnsCreatedResult()
    {
        _mockService.Setup(s => s.Create(It.IsAny<Incidence>()));

        var result = _controller.Create(_createRequest);

        Assert.IsInstanceOfType(result, typeof(CreatedResult));
    }

    [TestMethod]
    public void Create_WithValidRequest_CallsServiceCreate()
    {
        _mockService.Setup(s => s.Create(It.Is<Incidence>(i =>
            i.Description == _createRequest.Description &&
            i.AttractionId == _createRequest.AttractionId)));

        _controller.Create(_createRequest);

        _mockService.Verify(s => s.Create(It.IsAny<Incidence>()), Times.Once);
    }

    [TestMethod]
    public void Remove_WithExistingId_ReturnsOkResult()
    {
        _mockService.Setup(s => s.Delete(1));

        var result = _controller.Remove(1);

        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public void Remove_WithExistingId_CallsServiceDelete()
    {
        _mockService.Setup(s => s.Delete(1));

        _controller.Remove(1);

        _mockService.Verify(s => s.Delete(1), Times.Once);
    }

    [TestMethod]
    public void GetById_WithExistingId_ReturnsOkResult()
    {
        _mockService.Setup(s => s.GetById(1)).Returns(_incidence);

        var result = _controller.GetById(1);

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void GetById_WithExistingId_ReturnsResponseWithCorrectId()
    {
        _mockService.Setup(s => s.GetById(1)).Returns(_incidence);

        var result = _controller.GetById(1) as OkObjectResult;
        var response = result?.Value as GetIncidenceResponse;

        Assert.AreEqual(_incidence.Id, response?.Id);
    }

    [TestMethod]
    public void GetById_WithExistingId_ReturnsResponseWithCorrectDescription()
    {
        _mockService.Setup(s => s.GetById(1)).Returns(_incidence);

        var result = _controller.GetById(1) as OkObjectResult;
        var response = result?.Value as GetIncidenceResponse;

        Assert.AreEqual(_incidence.Description, response?.Description);
    }

    [TestMethod]
    public void GetById_WithExistingId_ReturnsResponseWithCorrectAttractionId()
    {
        _mockService.Setup(s => s.GetById(1)).Returns(_incidence);

        var result = _controller.GetById(1) as OkObjectResult;
        var response = result?.Value as GetIncidenceResponse;

        Assert.AreEqual(_incidence.AttractionId, response?.AttractionId);
    }

    [TestMethod]
    public void GetById_WithExistingId_ReturnsResponseWithCorrectActiveStatus()
    {
        _mockService.Setup(s => s.GetById(1)).Returns(_incidence);

        var result = _controller.GetById(1) as OkObjectResult;
        var response = result?.Value as GetIncidenceResponse;

        Assert.AreEqual(_incidence.Active, response?.Active);
    }

    [TestMethod]
    public void GetById_WithNonExistentId_ReturnsNotFoundResult()
    {
        _mockService.Setup(s => s.GetById(999)).Returns((Incidence?)null);

        var result = _controller.GetById(999);

        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }

    [TestMethod]
    public void GetAll_WithIncidences_ReturnsOkResult()
    {
        var incidences = new List<Incidence> { _incidence };
        _mockService.Setup(s => s.GetAll()).Returns(incidences);

        var result = _controller.GetAll();

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void GetAll_WithMultipleIncidences_ReturnsCorrectCount()
    {
        var incidences = new List<Incidence>
        {
            _incidence,
            new Incidence { Id = 2, Description = "Second", AttractionId = 2 }
        };
        _mockService.Setup(s => s.GetAll()).Returns(incidences);

        var result = _controller.GetAll() as OkObjectResult;
        var responses = (result?.Value as IQueryable<GetIncidenceResponse>)?.ToList();

        Assert.AreEqual(2, responses?.Count);
    }

    [TestMethod]
    public void GetAll_WithEmptyList_ReturnsEmptyList()
    {
        _mockService.Setup(s => s.GetAll()).Returns([]);

        var result = _controller.GetAll() as OkObjectResult;
        var responses = (result?.Value as IQueryable<GetIncidenceResponse>)?.ToList();

        Assert.AreEqual(0, responses?.Count);
    }

    [TestMethod]
    public void Update_WithValidData_ReturnsOkResult()
    {
        _mockService.Setup(s => s.Update(It.IsAny<Incidence>()));

        var result = _controller.Update(1, _createRequest);

        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public void Update_WithValidData_CallsServiceUpdate()
    {
        _mockService.Setup(s => s.Update(It.Is<Incidence>(i =>
            i.Id == 1 &&
            i.Description == _createRequest.Description)));

        _controller.Update(1, _createRequest);

        _mockService.Verify(s => s.Update(It.IsAny<Incidence>()), Times.Once);
    }

    [TestMethod]
    public void Update_WithId_SetsCorrectIdInEntity()
    {
        Incidence? capturedIncidence = null;
        _mockService.Setup(s => s.Update(It.IsAny<Incidence>()))
            .Callback<Incidence>(i => capturedIncidence = i);

        _controller.Update(5, _createRequest);

        Assert.AreEqual(5, capturedIncidence?.Id);
    }

    [TestMethod]
    public void GetActiveIncidences_WithActiveIncidences_ReturnsOkResult()
    {
        var activeIncidences = new List<Incidence> { _incidence };
        _mockService.Setup(s => s.GetActiveIncidences()).Returns(activeIncidences);

        var result = _controller.GetActiveIncidences();

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void GetActiveIncidences_WithMultipleActive_ReturnsCorrectCount()
    {
        var activeIncidences = new List<Incidence>
        {
            new Incidence { Id = 1, Description = "Active 1", Active = true, AttractionId = 1 },
            new Incidence { Id = 2, Description = "Active 2", Active = true, AttractionId = 1 }
        };
        _mockService.Setup(s => s.GetActiveIncidences()).Returns(activeIncidences);

        var result = _controller.GetActiveIncidences() as OkObjectResult;
        var responses = (result?.Value as IQueryable<GetIncidenceResponse>)?.ToList();

        Assert.AreEqual(2, responses?.Count);
    }

    [TestMethod]
    public void GetActiveIncidences_WithMultipleActive_AllAreActive()
    {
        var activeIncidences = new List<Incidence>
        {
            new Incidence { Id = 1, Description = "Active 1", Active = true, AttractionId = 1 },
            new Incidence { Id = 2, Description = "Active 2", Active = true, AttractionId = 1 }
        };
        _mockService.Setup(s => s.GetActiveIncidences()).Returns(activeIncidences);

        var result = _controller.GetActiveIncidences() as OkObjectResult;
        var responses = (result?.Value as IQueryable<GetIncidenceResponse>)?.ToList();

        Assert.IsTrue(responses?.All(r => r.Active));
    }

    [TestMethod]
    public void GetActiveIncidences_WithNoActiveIncidences_ReturnsEmptyList()
    {
        _mockService.Setup(s => s.GetActiveIncidences()).Returns([]);

        var result = _controller.GetActiveIncidences() as OkObjectResult;
        var responses = (result?.Value as IQueryable<GetIncidenceResponse>)?.ToList();

        Assert.AreEqual(0, responses?.Count);
    }

    [TestMethod]
    public void GetByAttractionId_WithIncidences_ReturnsOkResult()
    {
        var incidences = new List<Incidence> { _incidence };
        _mockService.Setup(s => s.GetByAttractionId(1)).Returns(incidences);

        var result = _controller.GetByAttractionId(1);

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void GetByAttractionId_WithMultipleIncidences_ReturnsCorrectCount()
    {
        var incidences = new List<Incidence>
        {
            new Incidence { Id = 1, Description = "Inc 1", AttractionId = 1, Active = true },
            new Incidence { Id = 2, Description = "Inc 2", AttractionId = 1, Active = false }
        };
        _mockService.Setup(s => s.GetByAttractionId(1)).Returns(incidences);

        var result = _controller.GetByAttractionId(1) as OkObjectResult;
        var responses = (result?.Value as IQueryable<GetIncidenceResponse>)?.ToList();

        Assert.AreEqual(2, responses?.Count);
    }

    [TestMethod]
    public void GetByAttractionId_WithMultipleIncidences_AllHaveCorrectAttractionId()
    {
        var incidences = new List<Incidence>
        {
            new Incidence { Id = 1, Description = "Inc 1", AttractionId = 1, Active = true },
            new Incidence { Id = 2, Description = "Inc 2", AttractionId = 1, Active = false }
        };
        _mockService.Setup(s => s.GetByAttractionId(1)).Returns(incidences);

        var result = _controller.GetByAttractionId(1) as OkObjectResult;
        var responses = (result?.Value as IQueryable<GetIncidenceResponse>)?.ToList();

        Assert.IsTrue(responses?.All(r => r.AttractionId == 1));
    }

    [TestMethod]
    public void GetByAttractionId_WithNoIncidences_ReturnsEmptyList()
    {
        _mockService.Setup(s => s.GetByAttractionId(999)).Returns([]);

        var result = _controller.GetByAttractionId(999) as OkObjectResult;
        var responses = (result?.Value as IQueryable<GetIncidenceResponse>)?.ToList();

        Assert.AreEqual(0, responses?.Count);
    }

    [TestMethod]
    public void DeactivateIncidence_WithExistingId_ReturnsOkResult()
    {
        _mockService.Setup(s => s.DeactivateIncidence(1));

        var result = _controller.DeactivateIncidence(1);

        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public void DeactivateIncidence_WithExistingId_CallsServiceDeactivate()
    {
        _mockService.Setup(s => s.DeactivateIncidence(1));

        _controller.DeactivateIncidence(1);

        _mockService.Verify(s => s.DeactivateIncidence(1), Times.Once);
    }

    [TestMethod]
    public void ActivateIncidence_WithExistingId_ReturnsOkResult()
    {
        _mockService.Setup(s => s.ActivateIncidence(1));

        var result = _controller.ActivateIncidence(1);

        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public void ActivateIncidence_WithExistingId_CallsServiceActivate()
    {
        _mockService.Setup(s => s.ActivateIncidence(1));

        _controller.ActivateIncidence(1);

        _mockService.Verify(s => s.ActivateIncidence(1), Times.Once);
    }

    [TestMethod]
    public void GetById_WithIncidenceWithAttraction_ReturnsAttractionName()
    {
        var incidenceWithAttraction = new Incidence
        {
            Id = 1,
            Description = "Test",
            AttractionId = 1,
            Active = true,
            AttractionName = "Roller Coaster"
        };
        _mockService.Setup(s => s.GetById(1)).Returns(incidenceWithAttraction);

        var result = _controller.GetById(1) as OkObjectResult;
        var response = result?.Value as GetIncidenceResponse;

        Assert.AreEqual("Roller Coaster", response?.AttractionName);
    }

    [TestMethod]
    public void GetById_WithIncidenceWithoutAttraction_ReturnsNullAttractionName()
    {
        _mockService.Setup(s => s.GetById(1)).Returns(_incidence);

        var result = _controller.GetById(1) as OkObjectResult;
        var response = result!.Value as GetIncidenceResponse;

        Assert.AreEqual(response!.AttractionName, string.Empty);
    }

    [TestMethod]
    public void CreateIncidenceRequest_ToEntity_SetsDefaultId()
    {
        var request = new CreateIncidenceRequest
        {
            Description = "New incidence",
            AttractionId = 5
        };

        var entity = request.ToEntity();

        Assert.AreEqual(0, entity.Id);
    }

    [TestMethod]
    public void CreateIncidenceRequest_ToEntity_SetsDescription()
    {
        var request = new CreateIncidenceRequest
        {
            Description = "New incidence",
            AttractionId = 5,
        };

        var entity = request.ToEntity();

        Assert.AreEqual("New incidence", entity.Description);
    }

    [TestMethod]
    public void CreateIncidenceRequest_ToEntity_SetsAttractionId()
    {
        var request = new CreateIncidenceRequest
        {
            Description = "New incidence",
            AttractionId = 5,
        };

        var entity = request.ToEntity();

        Assert.AreEqual(5, entity.AttractionId);
    }

    [TestMethod]
    public void CreateIncidenceRequest_ToEntity_ActiveStatusTrue()
    {
        var request = new CreateIncidenceRequest
        {
            Description = "New incidence",
            AttractionId = 5
        };

        var entity = request.ToEntity();

        Assert.IsTrue(entity.Active);
    }

    [TestMethod]
    public void CreateIncidenceRequest_ToEntityWithId_SetsCorrectId()
    {
        var request = new CreateIncidenceRequest
        {
            Description = "Update incidence",
            AttractionId = 3,
        };

        var entity = request.ToEntity(10);

        Assert.AreEqual(10, entity.Id);
    }

    [TestMethod]
    public void GetIncidenceResponse_FromIncidence_MapsId()
    {
        var incidence = new Incidence
        {
            Id = 5,
            Description = "Critical issue",
            AttractionId = 3,
            Active = false,
            AttractionName = "Space Mountain"
        };

        var response = new GetIncidenceResponse(incidence);

        Assert.AreEqual(5, response.Id);
    }

    [TestMethod]
    public void GetIncidenceResponse_FromIncidence_MapsDescription()
    {
        var incidence = new Incidence
        {
            Id = 5,
            Description = "Critical issue",
            AttractionId = 3,
            Active = false,
            AttractionName = "Space Mountain"
        };

        var response = new GetIncidenceResponse(incidence);

        Assert.AreEqual("Critical issue", response.Description);
    }

    [TestMethod]
    public void GetIncidenceResponse_FromIncidence_MapsAttractionId()
    {
        var incidence = new Incidence
        {
            Id = 5,
            Description = "Critical issue",
            AttractionId = 3,
            Active = false,
            AttractionName = "Space Mountain"
        };

        var response = new GetIncidenceResponse(incidence);

        Assert.AreEqual(3, response.AttractionId);
    }

    [TestMethod]
    public void GetIncidenceResponse_FromIncidence_MapsActiveStatus()
    {
        var incidence = new Incidence
        {
            Id = 5,
            Description = "Critical issue",
            AttractionId = 3,
            Active = false,
            AttractionName = "Space Mountain"
        };

        var response = new GetIncidenceResponse(incidence);

        Assert.IsFalse(response.Active);
    }

    [TestMethod]
    public void GetIncidenceResponse_FromIncidence_MapsAttractionName()
    {
        var incidence = new Incidence
        {
            Id = 5,
            Description = "Critical issue",
            AttractionId = 3,
            Active = false,
            AttractionName = "Space Mountain"
        };

        var response = new GetIncidenceResponse(incidence);

        Assert.AreEqual("Space Mountain", response.AttractionName);
    }
}
