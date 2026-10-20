using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Tests;

public static class SqlContextInMemoryCreate
{
    public static SqlContext Create()
    {
        var options = new DbContextOptionsBuilder<SqlContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new SqlContext(options);
    }
}
