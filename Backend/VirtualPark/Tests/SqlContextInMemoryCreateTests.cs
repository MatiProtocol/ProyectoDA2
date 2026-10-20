using DataAccess;

namespace Tests;
[TestClass]
public class SqlContextInMemoryCreateTests
{
    [TestMethod]
    public void Create_ReturnsSqlContext()
    {
        var result = SqlContextInMemoryCreate.Create();

        Assert.IsInstanceOfType<SqlContext>(result);
    }
}
