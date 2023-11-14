using LogisticMVC.DTOs.Module;
using static LogisticMVC.DTOs.AuthDTO.AuthDTO;

namespace LogisticMVC.Models.ModuleModels
{
    public class ParentAndChildViewModel
    {
        public string id { get; set; }
        public string moduleName { get; set; }
        public string moduleNameEn { get; set; }
        public string parentId { get; set; }
        public int screenOrder { get; set; }
        public bool IsSelected { get; set; }
        public Dictionary<GetAllModulesDTO.Root, List<GetAllModulesDTO.Root>> userModels { get; set; }
        public UserInfo UserInfo { get; set; } // UserInfo'yu ekleyin
    }
}
