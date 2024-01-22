using awesum.server.Model;

namespace csharp.Controllers;

public class PullAppRequest
{
    public int AppId { get; set; }
    public DateTime lastModified { get; set; }
    public int Version { get; set; }
}