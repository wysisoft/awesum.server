using awesum.server.Model;

namespace csharp.Controllers;

internal class PushDatabaseItemResponse
{
    public bool RequiresForce { get; set; }
    public int DatabaseItemId { get; set; }
}