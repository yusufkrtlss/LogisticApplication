using LogisticMVC.DTOs.Module;
using LogisticMVC.Models.ModuleModels;

namespace LogisticMVC.Models.CompanyModel
{
    public class CompanyViewModel
    {
        public string id { get; set; }
        public string companyName { get; set; }
        public string taxNumber { get; set; }
        public string address { get; set; }
        public int firmNumber { get; set; }
        public bool IsSelected { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
