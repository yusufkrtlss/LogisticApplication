using Microsoft.AspNetCore.Mvc.Rendering;

namespace LogisticMVC.Models.UserModels
{
    public class UserSelections
    {
        public List<SelectListItem> Companies { get; set; }
        public List<SelectListItem> Roles { get; set; }
    }

}
