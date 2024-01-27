using awesum.server.Model;

namespace csharp.Controllers;

internal class PullDatabaseItemResponse
{
    public DatabaseItem DatabaseItem { get; set; }  = new DatabaseItem();
}