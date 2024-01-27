using awesum.server.Model;

namespace csharp.Controllers;

public class PullDatabaseItemRequest
{    
    public bool IsLeader { get; set; }
    public int AppId { get; set; }
    public int DatabaseId { get; set; }
    public DatabaseItem DatabaseItem { get; set; } = new DatabaseItem();

}