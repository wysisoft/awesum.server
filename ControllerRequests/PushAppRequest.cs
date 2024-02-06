using awesum.server.Model;

namespace csharp.Controllers;

public class PushAppRequest
{
    public App App { get; set; } = new App();
    public bool Force { get; set; }
}
