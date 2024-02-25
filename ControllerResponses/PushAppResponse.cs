using awesum.server.Model;

namespace csharp.Controllers;

internal class PushAppResponse
{
    public bool RequiresForce { get; set; }
    public int AppId { get; set; }
}