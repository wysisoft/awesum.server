using awesum.server.Model;

namespace csharp.Controllers;

public class PullDatabaseRequest
{    
    public bool IsLeader { get; set; }
    public int AppId { get; set; }
    public Database Database { get; set; } = default!;
}