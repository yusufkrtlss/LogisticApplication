using LogisticMVC.DTOs.Roles;
using LogisticMVC.Models.CompanyModel;
using LogisticMVC.Models.ModuleModels;
using static LogisticMVC.DTOs.Roles.GetAllRolesDTO;

namespace LogisticMVC.Models.RolesModel
{
    public class RoleViewModel
    {
        public string id { get; set; }
        public string roleName { get; set; }
        public string? roleNameEn { get; set; }
        public bool isAdmin { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
