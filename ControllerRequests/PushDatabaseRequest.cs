using awesum.server.Model;

namespace csharp.Controllers;

public class PushDatabaseRequest
{
    public bool IsLeader { get; set; }
    public Database Database { get; set; } = new Database();
    public bool Force { get; set; }
}