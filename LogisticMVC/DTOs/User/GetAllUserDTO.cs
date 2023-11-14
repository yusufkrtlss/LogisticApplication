namespace LogisticMVC.DTOs.User
{
    public class GetAllUserDTO
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
            public List<RoleModule> roleModules { get; set; }
            public List<string> userModules { get; set; }
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
            public List<RoleModule> roleModules { get; set; }
            public List<string> userRoles { get; set; }
        }

        public class RoleModule
        {
            public string id { get; set; }
            public string roleId { get; set; }
            public string moduleId { get; set; }
            public string companyId { get; set; }
            public bool isDeleted { get; set; }
            public DateTime lastUpdated { get; set; }
            public string lastUpdatedBy { get; set; }
            public Company company { get; set; }
            public string module { get; set; }
            public string role { get; set; }
        }

        public class Root
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }  // Şifreyi API'den aldığınızda burası doldurulacak.
            public string FullName { get; set; }
            public bool IsActive { get; set; }
            public string Email { get; set; }
            public string? RefreshToken { get; set; }  // Nullable olup olmaması API'den gelen veriye göre belirlenmeli.
            public DateTime? RefreshTokenEndDate { get; set; }  // Nullable olup olmaması API'den gelen veriye göre belirlenmeli.
            public bool? IsDeleted { get; set; }  // Nullable olup olmaması API'den gelen veriye göre belirlenmeli.
            public DateTime? LastUpdated { get; set; }  // Nullable olup olmaması API'den gelen veriye göre belirlenmeli.
            public Guid? LastUpdatedBy { get; set; }  // Nullable olup olmaması API'den gelen veriye göre belirlenmeli.

            public List<UserCompany> UserCompanies { get; set; } = new List<UserCompany>();
            public List<UserModule> UserModules { get; set; } = new List<UserModule>();
            public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
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
            public Module module { get; set; }
            public string user { get; set; }
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
            public Role role { get; set; }
            public string user { get; set; }
        }


    }
}
