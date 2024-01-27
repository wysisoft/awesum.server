using awesum.server.Model;

namespace csharp.Controllers;

internal class PullDatabaseResponse
{
    public Database Database { get; set; } = new Database();
    public List<DatabaseType> Types { get; set; } = new List<DatabaseType>();
}