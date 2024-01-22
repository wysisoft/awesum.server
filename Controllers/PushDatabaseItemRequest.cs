using awesum.server.Model;

namespace csharp.Controllers;

public class PushDatabaseItemRequest
{
    public DatabaseItem DatabaseItem { get; set; }
    public bool Force { get; set; }
}