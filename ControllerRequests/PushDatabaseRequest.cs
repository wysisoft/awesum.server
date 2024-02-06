using awesum.server.Model;

namespace csharp.Controllers;

public class PushDatabaseRequest
{
    public Database Database { get; set; } = new Database();
    public bool Force { get; set; }
}