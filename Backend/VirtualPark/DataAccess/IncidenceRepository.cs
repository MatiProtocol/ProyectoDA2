using Abstraction;
using Domain;

namespace DataAccess;

public class IncidenceRepository(SqlContext context) : Repository<Incidence>(context), IIncidenceRepository
{
    public IEnumerable<Incidence> GetActiveIncidences()
    {
        return Query().Where(i => i.Active);
    }

    public IEnumerable<Incidence> GetByAttractionId(int attractionId)
    {
        return Query().Where(i => i.AttractionId == attractionId);
    }
}
