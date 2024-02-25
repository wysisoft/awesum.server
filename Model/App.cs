using System;
using System.Collections.Generic;

namespace awesum.server.Model;

public partial class App
{
    public string ManualId { get; set; } = "";

    public string Email { get; set; } = "";

    public string Loginid { get; set; } = "";

    public string Name { get; set; } = "";

    public DateTime LastModified { get; set; } = DateTime.Parse("1900-01-01");

    public string HomePageIcon { get; set; } = "";

    public bool Deleted { get; set; } = false;

    public int Version { get; set; } = 0;

    public bool AllowedToInitiateFollows { get; set; } = false;

    public Guid UniqueId { get; set; } = Guid.Empty;

    public string AuthenticationType { get; set; } = "";

    public int Id { get; set; } = 0;
}
