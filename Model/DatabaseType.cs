using System;
using System.Collections.Generic;

namespace awesum.server.Model;

public partial class DatabaseType
{
    public int Id { get; set; }

    public string? Type { get; set; }

    public int? DatabaseId { get; set; }

    public DateTime? LastModified { get; set; }

    public int? Version { get; set; }

    public int? Order { get; set; }

    public string? Loginid { get; set; }

    public string? UniqueId { get; set; }

    public string? DatabaseGroup { get; set; }

    public int? AppId { get; set; }

    public string? AppUniqueId { get; set; }
}
