using awesum.server.Model;

namespace csharp.Controllers;

public class PushDatabaseUnitRequest
{
    public DatabaseUnit DatabaseUnit { get; set; } = new DatabaseUnit();
    public bool Force { get; set; }
}
