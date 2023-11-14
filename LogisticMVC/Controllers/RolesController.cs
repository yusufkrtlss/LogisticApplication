using LogisticMVC.DTOs.APISettings;
using LogisticMVC.DTOs.AuthDTO;
using LogisticMVC.DTOs.Roles;
using LogisticMVC.Models.CompanyModel;
using LogisticMVC.Models.ModuleModels;
using LogisticMVC.Models.RolesModel;
using LogisticMVC.Models.UserModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using static LogisticMVC.DTOs.User.GetAllUserDTO;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace LojistikUygulamasi.Controllers
{
    public class RolesController : Controller
    {
        private readonly ApiSettings _apiSettings;
        private readonly IHttpClientFactory _httpClientFactory;


        public RolesController(ApiSettings apiSettings, IHttpClientFactory httpClientFactory)
        {
            _apiSettings = apiSettings;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<ActionResult> GetRoles()
        {
            string token = HttpContext.Request.Cookies["Token"];
            List<RoleViewModel> roles = new List<RoleViewModel>();

            using (var client = _httpClientFactory.CreateClient())
            {
                var apiUrl = _apiSettings.ApiUrl;
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                try
                {
                    var response = await client.GetStringAsync($"{apiUrl}/roles");
                    var roleResponse = JsonConvert.DeserializeObject<List<RoleViewModel>>(response);

                    foreach (var roleDTO in roleResponse)
                    {
                        if (roleDTO.IsDeleted != true)
                        {
                            // UserDTO verilerini UserViewModel'e dönüştür
                            var roleViewModel = new RoleViewModel
                            {
                                id = roleDTO.id,
                                roleName = roleDTO.roleName,
                                roleNameEn = roleDTO.roleNameEn,
                                isAdmin = roleDTO.isAdmin,
                            };

                            roles.Add(roleViewModel);
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata oluştu: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Bir hata oluştu, lütfen daha sonra tekrar deneyin.");
                }
            }

            return View(roles);
        }

        


        // EditRole/5
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            try
            {
                string token = HttpContext.Request.Cookies["Token"];

                using (var client = _httpClientFactory.CreateClient())
                {
                    var apiUrl = $"{_apiSettings.ApiUrl}/roles/{id}";

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {

                        var content = await response.Content.ReadAsStringAsync();
                        var roles = JsonConvert.DeserializeObject<EditRoleViewModel>(content);

                        //// Şirketlerin ve modüllerin listesini API'den al
                        //var companiesResponse = await client.GetAsync($"{_apiSettings.ApiUrl}/companies");
                        //var modulesResponse = await client.GetAsync($"{_apiSettings.ApiUrl}/modules");

                        //if (companiesResponse.IsSuccessStatusCode && modulesResponse.IsSuccessStatusCode)
                        //{
                        //    var companiesContent = await companiesResponse.Content.ReadAsStringAsync();
                        //    var modulesContent = await modulesResponse.Content.ReadAsStringAsync();

                        //    roles.Companies = JsonConvert.DeserializeObject<List<CompanyViewModel>>(companiesContent);
                        //    roles.Modules = JsonConvert.DeserializeObject<List<ParentAndChildViewModel>>(modulesContent);
                        //}

                        //Console.Write(roles);
                        return View(roles);
                    }

                    // Hata durumunda ModelState kullanılabilir.
                    TempData["ErrorMessage"] = "Rol getirilirken hata oluştu.";
                    return RedirectToAction("Index", "Roles");
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda isterseniz loglama yapabilirsiniz.
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                TempData["ErrorMessage"] = "Bir hata oluştu, lütfen daha sonra tekrar deneyin.";
                return RedirectToAction("Index", "Roles");
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


        private async Task<List<CompanyViewModel>> GetCompaniesAsync(string token)
        {
            List<CompanyViewModel> companies = new List<CompanyViewModel>();

            using (var client = _httpClientFactory.CreateClient())
            {
                var apiUrl = _apiSettings.ApiUrl;
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                try
                {
                    var response = await client.GetStringAsync($"{apiUrl}/companies");
                    companies = JsonConvert.DeserializeObject<List<CompanyViewModel>>(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata oluştu: {ex.Message}");
                    // Hata yönetimini burada gerçekleştirebilirsiniz.
                }
            }

            return companies;
        }

        private async Task<List<ParentAndChildViewModel>> GetModulesAsync(string token)
        {
            List<ParentAndChildViewModel> modules = new List<ParentAndChildViewModel>();

            using (var client = _httpClientFactory.CreateClient())
            {
                var apiUrl = _apiSettings.ApiUrl;
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                try
                {
                    var response = await client.GetStringAsync($"{apiUrl}/modules");
                    modules = JsonConvert.DeserializeObject<List<ParentAndChildViewModel>>(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata oluştu: {ex.Message}");
                    // Hata yönetimini burada gerçekleştirebilirsiniz.
                }
            }

            return modules;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            var roleModel = new RoleViewModel();
            return View(roleModel);
        }


        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleViewModel role)
        {
            // Yeni bir Guid oluştur
            role.id = Guid.NewGuid().ToString();

            // HTTP isteğinin Header'ına token ekleyin
            string token = HttpContext.Request.Cookies["Token"];

            // Diğer HTTP isteği için gerekli verileri ayarlayın
            var jsonCompany = JsonConvert.SerializeObject(role);
            var content = new StringContent(jsonCompany, Encoding.UTF8, "application/json");

            using (var client = _httpClientFactory.CreateClient())
            {
                // API endpoint'inizi oluşturun
                var apiUrl = _apiSettings.ApiUrl + "/roles";

                // HTTP isteğini yapılandırın
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                // POST isteği gönderin
                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    // Başarılı ekleme durumunda yönlendirme veya başka bir işlem yapabilirsiniz.
                    ViewData["SuccessMessage"] = "Rol başarıyla oluşturuldu.";
                }
                else
                {
                    // Ekleme başarısızsa hata mesajını göstermek için ModelState kullanılabilir.
                    ViewData["ErrorMessage"] = "Rol oluşturulurken hata oluştu.";
                }

                return View(role);
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            try
            {
                string token = HttpContext.Request.Cookies["Token"];

                using (var client = _httpClientFactory.CreateClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage response = await client.DeleteAsync($"{_apiSettings.ApiUrl}/roles/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Rol başarıyla silindi.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Rol silinirken hata oluştu.";
                    }

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
        public async Task<IActionResult> Index()
        {
            await GetRoles();
            return View();
        }
    }
}
