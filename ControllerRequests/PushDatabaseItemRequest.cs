using awesum.server.Model;

namespace csharp.Controllers;

public class PushDatabaseItemRequest
{
    public bool IsLeader { get; set; }
    public DatabaseItem DatabaseItem { get; set; } = new DatabaseItem();
    public bool Force { get; set; }
}