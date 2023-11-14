using System;
using System.Collections.Generic;

namespace LogisticApi.Models;

public partial class UserRole
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }

    public Guid CompanyId { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? LastUpdated { get; set; }

    public Guid? LastUpdatedBy { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
