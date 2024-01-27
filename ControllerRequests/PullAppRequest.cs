using awesum.server.Model;

namespace csharp.Controllers;

public class PullAppRequest
{
    public bool IsLeader { get; set; } = false;
    public App App { get; set; } = new App();
}