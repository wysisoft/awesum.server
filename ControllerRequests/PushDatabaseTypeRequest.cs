using awesum.server.Model;

namespace csharp.Controllers;

public class PushDatabaseTypeRequest
{
    public DatabaseType DatabaseType { get; set; } =    new DatabaseType();
    public bool Force { get; set; }
}
