using awesum.server.Model;

namespace csharp.Controllers;

internal class PushDatabaseUnitResponse
{
    public bool RequiresForce { get; set; }
    public int DatabaseUnitId { get; set; }
}