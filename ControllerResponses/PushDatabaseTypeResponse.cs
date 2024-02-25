using awesum.server.Model;

namespace csharp.Controllers;

internal class PushDatabaseTypeResponse
{
    public bool RequiresForce { get; set; }
    public int DatabaseTypeId { get; set; }
}