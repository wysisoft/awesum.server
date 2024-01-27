using System;
using System.Collections.Generic;

namespace awesum.server.Model;

public partial class DatabaseUnit
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? Order { get; set; }

    public DateTime? LastModified { get; set; }

    public bool? Deleted { get; set; }

    public int? DatabaseTypeId { get; set; }

    public int? Version { get; set; }

    public string? Loginid { get; set; }

    public string? UniqueId { get; set; }

    public int? DatabaseId { get; set; }

    public int? AppId { get; set; }
}
