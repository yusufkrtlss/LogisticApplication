using AutoMapper;
using LogisticApi.Models;
using LogisticMVC.DTOs.APISettings;
using LogisticMVC.Models.CompanyModel;
using LogisticMVC.Models.ModuleModels;
using LogisticMVC.Models.RoleModulesModels;
using LogisticMVC.Models.RolesModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using static LogisticMVC.DTOs.Roles.GetAllRolesDTO;
using static LogisticMVC.DTOs.User.GetAllUserDTO;

namespace LojistikUygulamasi.Controllers
{
    public class RoleModulesController : Controller
    {
        private readonly ApiSettings _apiSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;

        public RoleModulesController(ApiSettings apiSettings, IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _apiSettings = apiSettings;
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetRoleModulesByRoleId(string roleId)
        {
            try
            {
                string token = HttpContext.Request.Cookies["Token"];

                using (var client = _httpClientFactory.CreateClient())
                {
                    var apiUrl = $"{_apiSettings.ApiUrl}/RoleModules/ByRoleId/{roleId}";

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


                    var response = await client.GetAsync(apiUrl);
                    Console.WriteLine(response);
                    if (response.IsSuccessStatusCode)
                    {

                        var content = await response.Content.ReadAsStringAsync();

                        var roleModules = JsonConvert.DeserializeObject<List<RoleModuleViewModel>>(content);

                        var roleModuleViewModels = _mapper.Map<List<RoleModuleViewModel>>(roleModules);

                        return View(roleModuleViewModels);
                    }

                    TempData["ErrorMessage"] = "Modüller getirilirken hata oluştu.";
                    return RedirectToAction("Index", "Roles");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                TempData["ErrorMessage"] = "Bir hata oluştu, lütfen daha sonra tekrar deneyin.";
                return RedirectToAction("Index", "Roles");
            }
        }


        [HttpGet]
        public IActionResult CreateRoleModule(Guid roleId)
        {
            //string token = HttpContext.Request.Cookies["Token"];


            //List<CompanyViewModel> companies = await GetCompanies(token);


            //List<ModuleViewModel> modules = await GetModules(token);

            // ViewModel oluşturma
            var viewModel = new CreateRoleModuleViewModel
            {
                roleId = roleId,

            };

            return View(viewModel);
        }
        private async Task<List<CompanyViewModel>> GetCompanies(string token)
        {
            using (var companyClient = _httpClientFactory.CreateClient())
            {
                var companyApiUrl = _apiSettings.ApiUrl + "/companies";
                companyClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                try
                {
                    var companyResponse = await companyClient.GetStringAsync(companyApiUrl);
                    return JsonConvert.DeserializeObject<List<CompanyViewModel>>(companyResponse);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata oluştu: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Bir hata oluştu, lütfen daha sonra tekrar deneyin.");
                    return new List<CompanyViewModel>();
                }
            }
        }

        private async Task<List<ModuleViewModel>> GetModules(string token)
        {
            using (var moduleClient = _httpClientFactory.CreateClient())
            {
                var moduleApiUrl = _apiSettings.ApiUrl + "/modules";
                moduleClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                try
                {
                    var moduleResponse = await moduleClient.GetStringAsync(moduleApiUrl);
                    return JsonConvert.DeserializeObject<List<ModuleViewModel>>(moduleResponse);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata oluştu: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Bir hata oluştu, lütfen daha sonra tekrar deneyin.");
                    return new List<ModuleViewModel>();
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoleModule(CreateRoleModuleViewModel model)
        {

            try
            {

                var guidId = Guid.NewGuid();

                var roleModule = new CreateRoleModuleViewModel
                {
                    id = guidId,
                    roleId = model.roleId,
                    moduleId = model.moduleId,
                    companyId = model.companyId
                };


                Console.WriteLine($"RoleModule Properties: Id - {roleModule.id}, RoleId - {roleModule.roleId}, ModuleId - {roleModule.moduleId}, CompanyId - {roleModule.companyId}");

                using (var client = _httpClientFactory.CreateClient())
                {
                    var apiUrl = _apiSettings.ApiUrl + "/RoleModules";
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(roleModule), Encoding.UTF8, "application/json");


                    string token = HttpContext.Request.Cookies["Token"];
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);


                    Console.WriteLine($"API Endpoint: {apiUrl}");

                    Console.WriteLine($"Request Content: {await jsonContent.ReadAsStringAsync()}");

                    System.Diagnostics.Debugger.Break();


                    var response = await client.PostAsync(apiUrl, jsonContent);
                    Console.WriteLine(response);

                    if (response.IsSuccessStatusCode)
                    {

                        return RedirectToAction("Index", "RoleModules");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "RoleModule oluşturma sırasında bir hata oluştu.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Bir hata oluştu, lütfen daha sonra tekrar deneyin.");
            }


            return View("CreateRoleModule");
        }




        [HttpGet]
        public async Task<IActionResult> GetAllDataAndRoleModules(string roleId)
        {

            string token = HttpContext.Request.Cookies["Token"];

            // Companies için istek atma
            List<CompanyViewModel> companies = new List<CompanyViewModel>();
            using (var companyClient = _httpClientFactory.CreateClient())
            {
                var companyApiUrl = _apiSettings.ApiUrl + "/companies";
                companyClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                try
                {
                    var companyResponse = await companyClient.GetStringAsync(companyApiUrl);
                    companies = JsonConvert.DeserializeObject<List<CompanyViewModel>>(companyResponse);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata oluştu: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Bir hata oluştu, lütfen daha sonra tekrar deneyin.");
                }
            }

            // Modules için istek atma
            List<ModuleViewModel> modules = new List<ModuleViewModel>();
            using (var moduleClient = _httpClientFactory.CreateClient())
            {
                var moduleApiUrl = _apiSettings.ApiUrl + "/modules";
                moduleClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                try
                {
                    var moduleResponse = await moduleClient.GetStringAsync(moduleApiUrl);
                    modules = JsonConvert.DeserializeObject<List<ModuleViewModel>>(moduleResponse);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata oluştu: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Bir hata oluştu, lütfen daha sonra tekrar deneyin.");
                }
            }
            List<RoleModuleViewModel> roleModules = new List<RoleModuleViewModel>();

            using (var client = _httpClientFactory.CreateClient())
            {
                var apiUrl = $"{_apiSettings.ApiUrl}/RoleModules/ByRoleId/{roleId}";
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                try
                {
                    var response = await client.GetStringAsync(apiUrl);
                    roleModules = JsonConvert.DeserializeObject<List<RoleModuleViewModel>>(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata oluştu: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Bir hata oluştu, lütfen daha sonra tekrar deneyin.");
                }
            }



            var viewModel = new CompaniesAndModulesViewModel
            {
                Companies = companies,
                Modules = modules,
                RoleModules = roleModules
            };

            return View(viewModel);
        }

        // RoleModule /5
        [HttpGet]
        public async Task<IActionResult> EditRoleModule(string id)
        {
            try
            {
                string token = HttpContext.Request.Cookies["Token"];

                using (var client = _httpClientFactory.CreateClient())
                {
                    var apiUrl = $"{_apiSettings.ApiUrl}/rolemodules/{id}";

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {

                        var content = await response.Content.ReadAsStringAsync();
                        var roles = JsonConvert.DeserializeObject<EditRoleModuleViewModel>(content);
                        return View(roles);
                    }

                    TempData["ErrorMessage"] = "Rol getirilirken hata oluştu.";
                    return RedirectToAction("GetRoleModulesByRoleId", "RoleModules");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                TempData["ErrorMessage"] = "Bir hata oluştu, lütfen daha sonra tekrar deneyin.";
                return RedirectToAction("GetRoleModulesByRoleId", "RoleModules");
            }
        }



        [HttpPost]
        public async Task<IActionResult> EditRole(string id, EditRoleViewModel editedRole)
        {
            try
            {
                string token = HttpContext.Request.Cookies["Token"];

                using (var client = _httpClientFactory.CreateClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    string jsonUser = JsonConvert.SerializeObject(editedRole);

                    var content = new StringContent(jsonUser, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync($"{_apiSettings.ApiUrl}/roles/{id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Rol başarıyla güncellendi.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Rol güncellenirken hata oluştu.";
                    }


                    return View(editedRole);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                TempData["ErrorMessage"] = "Bir hata oluştu, lütfen daha sonra tekrar deneyin.";
                return RedirectToAction("Index", "Roles");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRoleModule(string id)
        {
            try
            {
                string token = HttpContext.Request.Cookies["Token"];

                using (var client = _httpClientFactory.CreateClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage response = await client.DeleteAsync($"{_apiSettings.ApiUrl}/rolemodules/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Rol Modül başarıyla silindi.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Rol Modül silinirken hata oluştu.";
                    }

                    return RedirectToAction("GetRoleModulesByRoleId", "RoleModules");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                TempData["ErrorMessage"] = "Bir hata oluştu, lütfen daha sonra tekrar deneyin.";
                return RedirectToAction("GetRoleModulesByRoleId", "RoleModules");
            }
        }
        public async Task<IActionResult> Index(string roleId)
        {
            await GetRoleModulesByRoleId(roleId);
            return View();
        }
    }
}
//    company = model.company != null
//? new CreateRoleModuleViewModel.Company
//{
//    id = model.company.id,
//    companyName = model.company.companyName,
//    taxNumber = model.company.taxNumber,
//    address = model.company.address,
//    firmNumber = model.company.firmNumber,
//    isDeleted = model.company.isDeleted,
//    lastUpdated = model.company.lastUpdated,
//    lastUpdatedBy = model.company.lastUpdatedBy
//}
//: null,
//    module = model.module != null
//? new CreateRoleModuleViewModel.Module
//{
//    id = model.module.id,
//    moduleName = model.module.moduleName,
//    // Other properties
//}
//: null,
//    role = model.role != null
//? new CreateRoleModuleViewModel.Role
//{
//    id = model.role.id,
//    roleName = model.role.roleName,
//    // Other properties
//}
//: null
//company = new CreateRoleModuleViewModel.Company
//{
//    // Map properties from the API response
//    id = model.company.id,
//    companyName = model.company.companyName,
//    taxNumber = model.company.taxNumber,
//    address = model.company.address,
//    firmNumber = model.company.firmNumber,
//    isDeleted = model.company.isDeleted,
//    lastUpdated = model.company.lastUpdated,
//    lastUpdatedBy = model.company.lastUpdatedBy
//},
//module = new CreateRoleModuleViewModel.Module
//{
//    // Map properties from the API response
//    id = model.module.id,
//    moduleName = model.module.moduleName,
//    // Other properties
//},
//role = new CreateRoleModuleViewModel.Role
//{
//    // Map properties from the API response
//    id = model.role.id,
//    roleName = model.role.roleName,
//    // Other properties
//}