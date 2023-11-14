using LogisticMVC.DTOs.APISettings;
using LogisticMVC.DTOs.User;
using LogisticMVC.Models.UserModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static LogisticMVC.DTOs.User.GetAllUserDTO;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using LogisticMVC.Models.CompanyModel;
using LogisticMVC.Models.ModuleModels;
using LogisticMVC.Models.RolesModel;
using NuGet.Common;
using LogisticApi.Models;

namespace LojistikUygulamasi.Controllers
{
    public class UserController : Controller
    {
        private readonly ApiSettings _apiSettings;
        private readonly IHttpClientFactory _httpClientFactory;


        public UserController(ApiSettings apiSettings, IHttpClientFactory httpClientFactory)
        {
            _apiSettings = apiSettings;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            string token = HttpContext.Request.Cookies["Token"];
            List<UserViewModel> users = new List<UserViewModel>();

            using (var client = _httpClientFactory.CreateClient())
            {
                var apiUrl = _apiSettings.ApiUrl;
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                try
                {
                    var response = await client.GetStringAsync($"{apiUrl}/users");
                    var userResponse = JsonConvert.DeserializeObject<List<GetAllUserDTO.Root>>(response);

                    foreach (var userDto in userResponse)
                    {
                        // UserDTO verilerini UserViewModel'e dönüştür
                        var userViewModel = new UserViewModel
                        {
                            Id = userDto.Id,
                            UserName = userDto.UserName,
                            FullName = userDto.FullName,
                            Password = userDto.Password,
                            Email = userDto.Email,
                            IsActive = userDto.IsActive

                        };

                        users.Add(userViewModel);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata oluştu: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Bir hata oluştu, lütfen daha sonra tekrar deneyin.");
                }
            }

            return View(users);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            var userModel = new UserViewModel();
            return View(userModel);
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserViewModel user)
        {
            // Yeni bir Guid oluştur
            user.Id = Guid.NewGuid().ToString();


            // Şifre Hashleme işlemi
            //using (SHA256 sha256 = SHA256.Create())
            //{
            //    byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(user.password));
            //    user.password = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            //}

            // isActive özelliğini otomatik olarak true yap
            user.IsActive = true;

            // HTTP isteğinin Header'ına token ekleyin
            string token = HttpContext.Request.Cookies["Token"];

            // Diğer HTTP isteği için gerekli verileri ayarlayın
            var jsonCompany = JsonConvert.SerializeObject(user);
            var content = new StringContent(jsonCompany, Encoding.UTF8, "application/json");

            using (var client = _httpClientFactory.CreateClient())
            {
                // API endpoint'inizi oluşturun
                var apiUrl = _apiSettings.ApiUrl + "/users";

                // HTTP isteğini yapılandırın
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                // POST isteği gönderin
                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    // Başarılı ekleme durumunda yönlendirme veya başka bir işlem yapabilirsiniz.
                    TempData["SuccessMessage"] = "Kullanıcı başarıyla oluşturuldu.";
                    return RedirectToAction("Index", "User");
                }

                // Ekleme başarısızsa hata mesajını göstermek için ModelState kullanılabilir.
                TempData["ErrorMessage"] = "Kullanıcı oluşturulurken hata oluştu.";
                return View(user);
            }
        }
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            try
            {
                // HTTP isteğinin Header'ına token ekleyin
                string token = HttpContext.Request.Cookies["Token"];

                using (var client = _httpClientFactory.CreateClient())
                {
                    // API endpoint'inizi oluşturun (silmek istediğiniz kullanıcının ID'sini ekleyin)
                    var apiUrl = $"{_apiSettings.ApiUrl}/users/{id}";

                    // HTTP isteğini yapılandırın
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    // GET isteği gönderin
                    var response = await client.GetAsync(apiUrl);
                    Console.WriteLine(response);
                    if (response.IsSuccessStatusCode)
                    {
                        // Kullanıcıyı doğrudan UserViewModel'e dönüştürün
                        var content = await response.Content.ReadAsStringAsync();
                        //Console.WriteLine(content);
                        //Console.WriteLine("\n"+id.ToString());
                        var user = JsonConvert.DeserializeObject<EditUserViewModel>(content);

                        //Console.WriteLine("\n" + user.ToString());
                        // Kullanıcı verisini kullanarak işlem yapabilirsiniz.
                        return View(user);
                    }

                    // Hata durumunda ModelState kullanılabilir.
                    TempData["ErrorMessage"] = "Kullanıcı getirilirken hata oluştu.";
                    return RedirectToAction("Index", "User");
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda isterseniz loglama yapabilirsiniz.
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                TempData["ErrorMessage"] = "Bir hata oluştu, lütfen daha sonra tekrar deneyin.";
                return RedirectToAction("Index", "User");
            }
        }



        [HttpPost]
        public async Task<IActionResult> EditUser(string id, EditUserViewModel editedUser)
        {
            try
            {
                // HTTP isteğinin Header'ına token ekleyin
                string token = HttpContext.Request.Cookies["Token"];

                using (var client = _httpClientFactory.CreateClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    string jsonUser = JsonConvert.SerializeObject(editedUser);

                    // HTTP isteği için gerekli content oluştur
                    var content = new StringContent(jsonUser, Encoding.UTF8, "application/json");

                    // PUT isteği gönder
                    HttpResponseMessage response = await client.PutAsync($"{_apiSettings.ApiUrl}/users/{id}", content);

                    Console.WriteLine(response.ToString());
                    if (response.IsSuccessStatusCode)
                    {
                        // Başarılı güncelleme durumunda yönlendirme veya başka bir işlem yapabilirsiniz.
                        TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi.";
                        return RedirectToAction("Index", "User");
                    }

                    // Güncelleme başarısızsa hata mesajını göstermek için ModelState kullanılabilir.
                    TempData["ErrorMessage"] = "Kullanıcı güncellenirken hata oluştu.";
                    return View(editedUser);
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda isterseniz loglama yapabilirsiniz.
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                TempData["ErrorMessage"] = "Bir hata oluştu, lütfen daha sonra tekrar deneyin.";
                return RedirectToAction("Index", "User");
            }
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                // HTTP isteğinin Header'ına token ekleyin
                string token = HttpContext.Request.Cookies["Token"];

                using (var client = _httpClientFactory.CreateClient())
                {
                    // API endpoint'inizi oluşturun (silmek istediğiniz kullanıcının ID'sini ekleyin)
                    var apiUrl = $"{_apiSettings.ApiUrl}/users/{id}";

                    // HTTP isteğini yapılandırın
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                    // DELETE isteği gönderin
                    var response = await client.DeleteAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        // Başarılı silme durumunda yönlendirme veya başka bir işlem yapabilirsiniz.
                        TempData["SuccessMessage"] = "Kullanıcı başarıyla silindi.";
                        return RedirectToAction("Index", "User");
                    }

                    // Silme başarısızsa hata mesajını göstermek için ModelState kullanılabilir.
                    TempData["ErrorMessage"] = "Kullanıcı silinirken hata oluştu.";
                    return RedirectToAction("Index", "User");
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda isterseniz loglama yapabilirsiniz.
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                TempData["ErrorMessage"] = "Bir hata oluştu, lütfen daha sonra tekrar deneyin.";
                return RedirectToAction("Index", "User");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserDefinitionForUserCompanyAndUserRoleAndUserModule()
        {
            try
            {
                string token = HttpContext.Request.Cookies["Token"];

                // Get User Companies - Roles - Modules

                var userCompanies = await GetUserCompanies(token);
                var userModules = await GetUserModules(token);
                var userRoles = await GetUserRoles(token);

                var viewModel = new GetUserCompaniesAndUserRolesAndUserModulesForUser
                {
                    UserCompanies = userCompanies,
                    UserModules = userModules,
                    UserRoles = userRoles
                };

                using (var client = _httpClientFactory.CreateClient())
                {
                    var apiUrl = $"{_apiSettings.ApiUrl}/users";
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    var response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        // Kullanıcıyı doğrudan UserViewModel'e dönüştürün
                        var content = await response.Content.ReadAsStringAsync();
                        var user = JsonConvert.DeserializeObject<EditUserViewModel>(content);

                        // Add the user to the ViewModel
                        //viewModel.User = user;

                        // Pass the ViewModel to the view
                        return View(viewModel);
                    }

                    // Hata durumunda ModelState kullanılabilir.
                    TempData["ErrorMessage"] = "Kullanıcı getirilirken hata oluştu.";
                    return RedirectToAction("Index", "User");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Bir hata oluştu, lütfen daha sonra tekrar deneyin.");
                return View("Error"); // Handle the error appropriately in your application
            }
        }

        [HttpGet]
        public async Task<IActionResult> DefinitionUser(string id)
        {
            try
            {
                string token = HttpContext.Request.Cookies["Token"];

                // Use utility methods to get companies, modules, and roles
                var companies = await GetCompanies(token);
                //var modules = await GetModules(token);
                //var roles = await GetRoles(token);

                // Create ViewModel
                var viewModel = new CompaniesAndRolesAndModulesForDefinitionUserMetot
                {
                    Companies = companies,
                    //Modules = modules,
                    //Roles = roles
                };

                // Continue with your existing code to get user information
                using (var client = _httpClientFactory.CreateClient())
                {
                    var apiUrl = $"{_apiSettings.ApiUrl}/users/{id}";
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    var response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        // Kullanıcıyı doğrudan UserViewModel'e dönüştürün
                        var content = await response.Content.ReadAsStringAsync();
                        var user = JsonConvert.DeserializeObject<EditUserViewModel>(content);

                        // Add the user to the ViewModel
                        //viewModel.User = user;

                        // Pass the ViewModel to the view
                        return View(viewModel);
                    }

                    // Hata durumunda ModelState kullanılabilir.
                    TempData["ErrorMessage"] = "Kullanıcı getirilirken hata oluştu.";
                    return RedirectToAction("Index", "User");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Bir hata oluştu, lütfen daha sonra tekrar deneyin.");
                return View("Error"); // Handle the error appropriately in your application
            }
        }


        public async Task<IActionResult> Index()
        {
            await GetAllUsers();
            return View();
        }


        // Usefully Metots
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

        private async Task<List<RoleViewModel>> GetRoles(string token)
        {
            using (var roleClient = _httpClientFactory.CreateClient())
            {
                var roleApiUrl = _apiSettings.ApiUrl + "/roles";
                roleClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                try
                {
                    var roleResponse = await roleClient.GetStringAsync(roleApiUrl);
                    return JsonConvert.DeserializeObject<List<RoleViewModel>>(roleResponse);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Hata oluştu: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Bir hata oluştu, lütfen daha sonra tekrar deneyin.");
                    return new List<RoleViewModel>();
                }
            }
        }

        private async Task<List<LogisticApi.Models.UserCompany>> GetUserCompanies(string token)
        {
            using (var companyClient = _httpClientFactory.CreateClient())
            {
                var companyApiUrl = _apiSettings.ApiUrl + "/companies";
                companyClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                try
                {
                    var companyResponse = await companyClient.GetStringAsync(companyApiUrl);
                    return JsonConvert.DeserializeObject<List<LogisticApi.Models.UserCompany>>(companyResponse);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata oluştu: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Bir hata oluştu, lütfen daha sonra tekrar deneyin.");
                    return new List<LogisticApi.Models.UserCompany>();
                }
            }
        }

        private async Task<List<LogisticApi.Models.UserRole>> GetUserRoles(string token)
        {
            using (var companyClient = _httpClientFactory.CreateClient())
            {
                var companyApiUrl = _apiSettings.ApiUrl + "/companies";
                companyClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                try
                {
                    var companyResponse = await companyClient.GetStringAsync(companyApiUrl);
                    return JsonConvert.DeserializeObject<List<LogisticApi.Models.UserRole>>(companyResponse);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata oluştu: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Bir hata oluştu, lütfen daha sonra tekrar deneyin.");
                    return new List<LogisticApi.Models.UserRole>();
                }
            }
        }

        private async Task<List<LogisticApi.Models.UserModule>> GetUserModules(string token)
        {
            using (var companyClient = _httpClientFactory.CreateClient())
            {
                var companyApiUrl = _apiSettings.ApiUrl + "/companies";
                companyClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                try
                {
                    var companyResponse = await companyClient.GetStringAsync(companyApiUrl);
                    return JsonConvert.DeserializeObject<List<LogisticApi.Models.UserModule>>(companyResponse);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata oluştu: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Bir hata oluştu, lütfen daha sonra tekrar deneyin.");
                    return new List<LogisticApi.Models.UserModule>();
                }
            }
        }
    }
}
