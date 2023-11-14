using LogisticMVC.DTOs.Roles;
using static LogisticMVC.DTOs.Roles.GetAllRolesDTO;

namespace LogisticMVC.Models.RolesModel
{
    public class RoleModuleViewModel
    {
        public string id { get; set; }
        public string roleId { get; set; }
        public string moduleId { get; set; }
        public string companyId { get; set; }
        public bool? isDeleted { get; set; }
        public DateTime? lastUpdated { get; set; }
        public string? lastUpdatedBy { get; set; }
        public Company Company { get; set; }
        public Module Module { get; set; }
        public string role { get; set; }
    }
}
