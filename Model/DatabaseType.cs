using System;
using System.Collections.Generic;

namespace awesum.server.Model;

public partial class DatabaseType
{
    public string Type { get; set; } = "";

    public int DatabaseId { get; set; } = 0;

    public DateTime LastModified { get; set; } = DateTime.Parse("1900-01-01");

    public int Version { get; set; } = 0;

    public int Order { get; set; } = 0;

    public string Loginid { get; set; } = "";

    public string DatabaseGroup { get; set; } = "";

    public int AppId { get; set; } = 0;

    public string AppUniqueId { get; set; } = "";

    public string DatabaseUniqueId { get; set; } = "";

    public Guid UniqueId { get; set; } = Guid.Empty;

    public int Id { get; set; } = 0;
}
