using awesum.server.Model;

namespace csharp.Controllers;

public class PullDatabaseTypeRequest
{    
    public bool IsLeader { get; set; }
    public int AppId { get; set; }
    public int DatabaseId { get; set; }
    public DatabaseType DatabaseType { get; set; } = new DatabaseType();
}