using awesum.server.Model;

namespace csharp.Controllers;

internal class PullDatabaseTypeResponse
{
    public DatabaseType DatabaseType { get; set; } = new DatabaseType();
    public List<DatabaseUnit> Units { get; set; } = new List<DatabaseUnit>();
}