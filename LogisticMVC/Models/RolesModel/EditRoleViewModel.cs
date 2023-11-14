using LogisticMVC.Models.CompanyModel;
using LogisticMVC.Models.ModuleModels;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static LogisticMVC.DTOs.Roles.GetAllRolesDTO;

namespace LogisticMVC.Models.RolesModel
{
    public class EditRoleViewModel
    {
        public string id { get; set; }
        public string roleName { get; set; }
        public string roleNameEn { get; set; }
        public bool isAdmin { get; set; }
        public bool? isDeleted { get; set; }
        public DateTime? lastUpdated { get; set; }
        public string? lastUpdatedBy { get; set; }
        //public List<RoleModule> roleModules { get; set; }
        //public List<UserRole> userRoles { get; set; }
    }
}
