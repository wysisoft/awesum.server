using awesum.server.Model;
using Microsoft.EntityFrameworkCore;
namespace awesum.server.Model;
public partial class AwesumContext : DbContext

{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyUtcDateTimeConverter();//Put before seed data and after model creation
    }
}