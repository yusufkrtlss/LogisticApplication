using LogisticMVC.Models.CompanyModel;
using LogisticMVC.Models.ModuleModels;

namespace LogisticMVC.Models.RoleModulesModels
{
    public class CombinedViewModel
    {
        public CreateRoleModuleViewModel CreateRoleModuleViewModel { get; set; }
        public CompanyViewModel CompanyViewModel { get; set; }
        public ModuleViewModel ModuleViewModel { get; set; }
    }
}
