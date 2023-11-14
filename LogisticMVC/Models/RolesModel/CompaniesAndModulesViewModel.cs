using LogisticMVC.Models.CompanyModel;
using LogisticMVC.Models.ModuleModels;

namespace LogisticMVC.Models.RolesModel
{
    public class CompaniesAndModulesViewModel
    {
        public string roleId { get; set; }
        public List<CompanyViewModel> Companies { get; set; }
        public List<ModuleViewModel> Modules { get; set; }
        public List<RoleModuleViewModel> RoleModules { get; set; }
    }
}
