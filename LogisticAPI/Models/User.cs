using System;
using System.Collections.Generic;

namespace LogisticApi.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public bool IsActive { get; set; }

    public string? Email { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenEndDate { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? LastUpdated { get; set; }

    public Guid? LastUpdatedBy { get; set; }

    public virtual ICollection<UserCompany> UserCompanies { get; set; } = new List<UserCompany>();

    public virtual ICollection<UserModule> UserModules { get; set; } = new List<UserModule>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
