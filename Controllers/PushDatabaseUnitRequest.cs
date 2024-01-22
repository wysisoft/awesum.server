using awesum.server.Model;

namespace csharp.Controllers;

public class PushDatabaseUnitRequest
{
    public DatabaseUnit DatabaseUnit { get; set; }
    public bool Force { get; set; }
}
