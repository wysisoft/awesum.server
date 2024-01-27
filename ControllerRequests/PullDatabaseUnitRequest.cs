using awesum.server.Model;

namespace csharp.Controllers;

public class PullDatabaseUnitRequest
{    
    public bool IsLeader { get; set; }
    public int AppId { get; set; }
    public int DatabaseId { get; set; }
    public DatabaseUnit DatabaseUnit { get; set; } = new DatabaseUnit();

}