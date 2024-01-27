using awesum.server.Model;

namespace csharp.Controllers;

public class PushDatabaseUnitRequest
{
    public bool IsLeader { get; set; }
    public DatabaseUnit DatabaseUnit { get; set; } = new DatabaseUnit();
    public bool Force { get; set; }
}
