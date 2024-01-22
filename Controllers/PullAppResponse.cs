using awesum.server.Model;

namespace csharp.Controllers;

internal class PullAppResponse
{
    public App App { get; set; }
    public List<int> Databases { get; set; }
}