using LogisticMVC.DTOs.APISettings;
using LogisticMVC.DTOs.AuthDTO;
using LogisticMVC.DTOs.Company;
using LogisticMVC.DTOs.Module;
using LogisticMVC.DTOs.User;
using LogisticMVC.Models.CompanyModel;
using LogisticMVC.Models.ModuleModels;
using LogisticMVC.Models.UserModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace LojistikUygulamasi.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ApiSettings _apiSettings;
        private readonly IHttpClientFactory _httpClientFactory;


        public CompanyController(ApiSettings apiSettings, IHttpClientFactory httpClientFactory)
        {
            _apiSettings = apiSettings;
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await GetCompanies();
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetCompanies()
        {
            string token = HttpContext.Request.Cookies["Token"];
            List<CompanyViewModel> companies = new List<CompanyViewModel>();

            using (var client = _httpClientFactory.CreateClient())
            {
                var apiUrl = _apiSettings.ApiUrl;
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                try
                {
                    var response = await client.GetStringAsync($"{apiUrl}/companies");
                    var companyResponse = JsonConvert.DeserializeObject<List<CompanyViewModel>>(response);

                    foreach (var companyDTO in companyResponse)
                    {
                        if (companyDTO.IsDeleted != true)
                        {
                            // UserDTO verilerini UserViewModel'e dönüştür
                            var companyViewModel = new CompanyViewModel
                            {
                                id = companyDTO.id,
                                companyName = companyDTO.companyName,
                                taxNumber = companyDTO.taxNumber,
                                firmNumber = companyDTO.firmNumber,
                                address = companyDTO.address,
                            };

                            companies.Add(companyViewModel);
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata oluştu: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Bir hata oluştu, lütfen daha sonra tekrar deneyin.");
                }
            }

            return View(companies);
        }
        [HttpGet]
        public IActionResult CreateCompany()
        {
            var companyModel = new CompanyViewModel();
            return View(companyModel);

        }
        [HttpPost]
        public async Task<IActionResult> CreateCompany(CompanyViewModel company)
        {
            // Yeni bir Guid oluştur
            company.id = Guid.NewGuid().ToString();

            // HTTP isteğinin Header'ına token ekleyin
            string token = HttpContext.Request.Cookies["Token"];

            // Diğer HTTP isteği için gerekli verileri ayarlayın
            var jsonCompany = JsonConvert.SerializeObject(company);
            var content = new StringContent(jsonCompany, Encoding.UTF8, "application/json");

            using (var client = _httpClientFactory.CreateClient())
            {
                // API endpoint'inizi oluşturun
                var apiUrl = _apiSettings.ApiUrl + "/companies";

                // HTTP isteğini yapılandırın
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                // POST isteği gönderin
                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    // Başarılı ekleme durumunda yönlendirme veya başka bir işlem yapabilirsiniz.
                    ViewData["SuccessMessage"] = "Şirket başarıyla oluşturuldu.";
                }
                else
                {
                    // Ekleme başarısızsa hata mesajını göstermek için ModelState kullanılabilir.
                    ViewData["ErrorMessage"] = "Şirket oluşturulurken hata oluştu.";
                }

                return View(company);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditCompany(string id)
        {
            try
            {
                string token = HttpContext.Request.Cookies["Token"];

                using (var client = _httpClientFactory.CreateClient())
                {
                    var apiUrl = $"{_apiSettings.ApiUrl}/companies/{id}";

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            
                    var response = await client.GetAsync(apiUrl);
                    Console.WriteLine(response);
                    if (response.IsSuccessStatusCode)
                    {
                    
                        var content = await response.Content.ReadAsStringAsync();
                    
                        var company = JsonConvert.DeserializeObject<CompanyViewModel>(content);

                     
                        return View(company);
                    }

                    // Hata durumunda ModelState kullanılabilir.
                    TempData["ErrorMessage"] = "Şirket getirilirken hata oluştu.";
                    return RedirectToAction("Index", "Company");
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda isterseniz loglama yapabilirsiniz.
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                TempData["ErrorMessage"] = "Bir hata oluştu, lütfen daha sonra tekrar deneyin.";
                return RedirectToAction("Index", "Company");
            }
        }



        [HttpPost]
        public async Task<IActionResult> EditCompany(string id, CompanyViewModel editedCompany)
        {
            try
            {
                string token = HttpContext.Request.Cookies["Token"];

                using (var client = _httpClientFactory.CreateClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    string jsonUser = JsonConvert.SerializeObject(editedCompany);

                    var content = new StringContent(jsonUser, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync($"{_apiSettings.ApiUrl}/companies/{id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Şirket başarıyla güncellendi.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Şirket güncellenirken hata oluştu.";
                    }

                  
                    return View(editedCompany);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                TempData["ErrorMessage"] = "Bir hata oluştu, lütfen daha sonra tekrar deneyin.";
                return RedirectToAction("Index", "Company");
            }
        }
       
        [HttpGet]
        public async Task<IActionResult> DeleteCompany(string id)
        {
            try
            {
                string token = HttpContext.Request.Cookies["Token"];

                using (var client = _httpClientFactory.CreateClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage response = await client.DeleteAsync($"{_apiSettings.ApiUrl}/companies/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Şirket başarıyla silindi.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Şirket silinirken hata oluştu.";
                    }

                    return RedirectToAction("Index", "Company");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                TempData["ErrorMessage"] = "Bir hata oluştu, lütfen daha sonra tekrar deneyin.";
                return RedirectToAction("Index", "Company");
            }
        }

    }
}

