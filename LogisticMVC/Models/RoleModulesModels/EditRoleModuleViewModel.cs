using LogisticApi.Models;

namespace LogisticMVC.Models.RoleModulesModels
{
    public class EditRoleModuleViewModel
    {
        public string id { get; set; }
        public string roleId { get; set; }
        public string moduleId { get; set; }
        public string companyId { get; set; }
        public bool? IsDeleted { get; set; }

        public DateTime? LastUpdated { get; set; }

        public Guid? LastUpdatedBy { get; set; }

        public virtual Company Company { get; set; } = null!;

        public virtual Module Module { get; set; } = null!;

        public virtual Role Role { get; set; } = null!;
        public List<Company> CompanyList { get; set; } = new List<Company>();
        public List<Module> ModuleList { get; set; } = new List<Module>();
    }
}
