using System;
using System.Collections.Generic;

namespace LogisticApi.Models;

public partial class UserSession
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public DateTime LoginDate { get; set; }

    public string ClientName { get; set; } = null!;

    public DateTime LastAccessDate { get; set; }

    public string Token { get; set; } = null!;
}
