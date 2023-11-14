using LogisticApi.Models;

namespace LogisticMVC.Models.UserModels
{
    public class GetUserCompaniesAndUserRolesAndUserModulesForUser
    {
        public List<UserCompany> UserCompanies { get; set; } = new List<UserCompany>();
        public List<UserModule> UserModules { get; set; } = new List<UserModule>();
        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
