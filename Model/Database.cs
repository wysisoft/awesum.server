using System;
using System.Collections.Generic;

namespace awesum.server.Model;

public partial class Database
{
    public int Id { get; set; }

    public int? AppId { get; set; }

    public string? Name { get; set; }

    public DateTime? Default { get; set; }

    public DateTime? LastModified { get; set; }

    public string? ManualId { get; set; }

    public Guid? UniqueId { get; set; }
}
