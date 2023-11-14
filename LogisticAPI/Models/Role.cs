using System;
using System.Collections.Generic;

namespace LogisticApi.Models;

public partial class Role
{
    public Guid Id { get; set; }

    public string RoleName { get; set; } = null!;

    public string? RoleNameEn { get; set; }

    public bool IsAdmin { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? LastUpdated { get; set; }

    public Guid? LastUpdatedBy { get; set; }

    public virtual ICollection<RoleModule> RoleModules { get; set; } = new List<RoleModule>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
