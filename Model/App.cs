using System;
using System.Collections.Generic;

namespace awesum.server.Model;

public partial class App
{
    public int Id { get; set; }

    public string? ManualId { get; set; }

    public string? Email { get; set; }

    public string? Loginid { get; set; }

    public string? Name { get; set; }

    public DateTime? LastModified { get; set; }

    public string? HomePageIcon { get; set; }

    public bool? Deleted { get; set; }

    public int? Version { get; set; }
}
