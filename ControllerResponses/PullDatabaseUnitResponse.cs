using awesum.server.Model;

namespace csharp.Controllers;

internal class PullDatabaseUnitResponse
{
    public DatabaseUnit DatabaseUnit { get; set; } = new DatabaseUnit();
    public List<DatabaseItem> Items { get; set; } = new List<DatabaseItem>();
}