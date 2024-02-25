using System;
using System.Collections.Generic;

namespace awesum.server.Model;

public partial class Database
{
    public string Name { get; set; } = "";

    public DateTime LastModified { get; set; } = DateTime.Parse("1900-01-01");

    public bool Deleted { get; set; } = false;

    public int Version { get; set; } = 0;

    public int Order { get; set; } = 0;

    public string Loginid { get; set; } = "";

    public string GroupName { get; set; } = "";

    public int AppId { get; set; } = 0;

    public string AppUniqueId { get; set; } = "";

    public Guid UniqueId { get; set; } = Guid.Empty;

    public int Id { get; set; } = 0;
}
