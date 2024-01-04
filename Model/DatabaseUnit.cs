using System;
using System.Collections.Generic;

namespace awesum.server.Model;

public partial class DatabaseUnit
{
    public int Id { get; set; }

    public int? Type { get; set; }

    public string? Name { get; set; }

    public int? Order { get; set; }

    public DateTime? LastModified { get; set; }

    public int? DatabaseId { get; set; }

    public int? AppId { get; set; }

    public Guid? UniqueId { get; set; }

    public bool? Deleted { get; set; }
}
