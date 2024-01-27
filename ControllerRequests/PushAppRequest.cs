using awesum.server.Model;

namespace csharp.Controllers;

public class PushAppRequest
{
    public bool IsLeader { get; set; }
    public App App { get; set; } = new App();
    public bool Force { get; set; }
}
