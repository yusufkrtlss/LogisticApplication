namespace LogisticMVC.DTOs.AuthDTO
{
    public class AuthDTO
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Data
        {
            public Token token { get; set; }
            public UserInfo userInfo { get; set; }
        }

        public class Root
        {
            public Data data { get; set; }
            public bool success { get; set; }
            public string message { get; set; }
        }

        public class Token
        {
            public string accessToken { get; set; }
            public DateTime expiration { get; set; }
            public string refreshToken { get; set; }
        }

        public class UserInfo
        {
            public string id { get; set; }
            public string userName { get; set; }
            public string password { get; set; }
            public string fullName { get; set; }
            public bool isActive { get; set; }
            public string email { get; set; }
            public string refreshToken { get; set; }
            public DateTime refreshTokenEndDate { get; set; }
            public object isDeleted { get; set; }
            public object lastUpdated { get; set; }
            public object lastUpdatedBy { get; set; }
            public List<object> userCompanies { get; set; }
            public List<UserModule> userModules { get; set; }
            public List<object> userRoles { get; set; }
        }

        public class UserModule
        {
            public string id { get; set; }
            public string userId { get; set; }
            public string moduleId { get; set; }
            public string companyId { get; set; }
            public object company { get; set; }
            public object module { get; set; }
            public object user { get; set; }
        }


    }
}
