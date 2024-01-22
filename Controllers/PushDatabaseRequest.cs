using awesum.server.Model;

namespace csharp.Controllers;

public class PushDatabaseRequest
{
    public Database Database { get; set; }
    public bool Force { get; set; }
}
