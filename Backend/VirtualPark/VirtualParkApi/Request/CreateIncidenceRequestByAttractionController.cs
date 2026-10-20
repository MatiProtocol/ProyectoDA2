using Domain;

namespace VirtualParkApi.Request;

public class CreateIncidenceRequestByAttractionController
{
    public required string Description { get; set; }

    public Incidence ToEntity(int attractionId)
    {
        return new Incidence
        {
            Description = Description,
            Active = true,
            AttractionId = attractionId
        };
    }
}
