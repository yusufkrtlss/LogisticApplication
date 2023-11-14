namespace LogisticMVC.Models.RolesModel
{
    public class CreateRoleViewModel
    {
        public string id { get; set; }
        public string roleName { get; set; }
        public string? roleNameEn { get; set; }
        public bool isAdmin { get; set; }
    }
}
