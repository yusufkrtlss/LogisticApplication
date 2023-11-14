using static LogisticMVC.DTOs.User.GetAllUserDTO;

namespace LogisticMVC.Models.UserModels
{
    public class EditUserViewModel
    {
        public Guid id { get; set; }

        public string userName { get; set; } = null!;

        public string password { get; set; } = null!;

        public string fullName { get; set; } = null!;

        public bool isActive { get; set; }

        public string? email { get; set; }

        public string? refreshToken { get; set; }

        public DateTime? refreshTokenEndDate { get; set; }

        public bool? isDeleted { get; set; }

        public DateTime? lastUpdated { get; set; }

        public Guid? lastUpdatedBy { get; set; }

        public virtual ICollection<UserCompany> userCompanies { get; set; } = new List<UserCompany>();

        public virtual ICollection<UserModule> userModules { get; set; } = new List<UserModule>();

        public virtual ICollection<UserRole> userRoles { get; set; } = new List<UserRole>();
    }
}
