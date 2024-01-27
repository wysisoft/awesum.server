using awesum.server.Model;

namespace csharp.Controllers;

internal class PullAppResponse
{
    public App App { get; set; }= new App();
    public List<Database> Databases { get; set; } = new List<Database>();
}