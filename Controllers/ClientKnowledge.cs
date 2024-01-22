using awesum.server.Model;

namespace csharp.Controllers;

public class PushDatabaseItem
{
    public App App { get; set; }
    public Database Database { get; set; }
    public DatabaseType DatabaseType { get; set; }
    public DatabaseUnit DatabaseUnit { get; set; }
    public DatabaseItem DatabaseItem { get; set; }
    public Follower Follower { get; set; }
    public bool Force { get; set; }
}