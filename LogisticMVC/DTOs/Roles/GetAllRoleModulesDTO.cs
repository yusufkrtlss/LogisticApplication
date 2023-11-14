namespace LogisticMVC.DTOs.Roles
{
    public class GetAllRoleModulesDTO
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
        public class Company
        {
            public string id { get; set; }
            public string companyName { get; set; }
            public string taxNumber { get; set; }
            public string address { get; set; }
            public int firmNumber { get; set; }
            public bool isDeleted { get; set; }
            public DateTime lastUpdated { get; set; }
            public string lastUpdatedBy { get; set; }
        }

        public class Module
        {
            public string id { get; set; }
            public string moduleName { get; set; }
            public string moduleNameEn { get; set; }
            public string parentId { get; set; }
            public int screenOrder { get; set; }
            public List<string> roleModules { get; set; }
            public List<UserModule> userModules { get; set; }
        }

        public class Role
        {
            public string id { get; set; }
            public string roleName { get; set; }
            public string roleNameEn { get; set; }
            public bool isAdmin { get; set; }
            public bool isDeleted { get; set; }
            public DateTime lastUpdated { get; set; }
            public string lastUpdatedBy { get; set; }
            public List<string> roleModules { get; set; }
            public List<UserRole> userRoles { get; set; }
        }

        public class Root
        {
            public string id { get; set; }
            public string roleId { get; set; }
            public string moduleId { get; set; }
            public string companyId { get; set; }
            public bool isDeleted { get; set; }
            public DateTime lastUpdated { get; set; }
            public string lastUpdatedBy { get; set; }
            public Company company { get; set; }
            public Module module { get; set; }
            public Role role { get; set; }
        }

        public class User
        {
            public string id { get; set; }
            public string userName { get; set; }
            public string password { get; set; }
            public string fullName { get; set; }
            public bool isActive { get; set; }
            public string email { get; set; }
            public string refreshToken { get; set; }
            public DateTime refreshTokenEndDate { get; set; }
            public bool isDeleted { get; set; }
            public DateTime lastUpdated { get; set; }
            public string lastUpdatedBy { get; set; }
            public List<UserCompany> userCompanies { get; set; }
            public List<string> userModules { get; set; }
            public List<string> userRoles { get; set; }
        }

        public class UserCompany
        {
            public string id { get; set; }
            public string userId { get; set; }
            public string companyId { get; set; }
            public bool isDeleted { get; set; }
            public DateTime lastUpdated { get; set; }
            public string lastUpdatedBy { get; set; }
            public Company company { get; set; }
            public string user { get; set; }
        }

        public class UserModule
        {
            public string id { get; set; }
            public string userId { get; set; }
            public string moduleId { get; set; }
            public string companyId { get; set; }
            public Company company { get; set; }
            public string module { get; set; }
            public User user { get; set; }
        }

        public class UserRole
        {
            public string id { get; set; }
            public string userId { get; set; }
            public string roleId { get; set; }
            public string companyId { get; set; }
            public bool isDeleted { get; set; }
            public DateTime lastUpdated { get; set; }
            public string lastUpdatedBy { get; set; }
            public Company company { get; set; }
            public string role { get; set; }
            public User user { get; set; }
        }


    }
}
