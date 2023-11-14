using LogisticMVC.DTOs.APISettings;
using LogisticMVC.DTOs.AuthDTO;
using LogisticMVC.DTOs.Module;
using LogisticMVC.Models.LoginModels;
using LogisticMVC.Models.ModuleModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LojistikUygulamasi.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;

        public LoginController(IHttpClientFactory httpClientFactory, ApiSettings apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiSettings;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var viewModel = new LoginViewModel();
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            try
            {
                var apiUrl = _apiSettings.ApiUrl;

                var values = new Dictionary<string, string>
                {
                    { "FirmNumber", model.FirmNumber.ToString() },
                    { "Username", model.Username },
                    { "Password", model.Password }
                };

                using (var client = _httpClientFactory.CreateClient())
                {
                    var content = new FormUrlEncodedContent(values);
                    var response = await client.PostAsync($"{apiUrl}/Auth", content);
                    var responseString = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        AuthDTO.Root authResponse = JsonConvert.DeserializeObject<AuthDTO.Root>(responseString);
                        var token = authResponse.data.token.accessToken;
                        // Login işlemi başarılı olduysa
                        //var viewModel = new ParentAndChildViewModel
                        //{
                        //    userModules = new Dictionary<GetAllModulesDTO.Root, List<GetAllModulesDTO.Root>>(),
                        //    UserInfo = authResponse.data.userInfo
                        //}; ;

                        string userModulsId = "";
                        foreach (var userModule in authResponse.data.userInfo.userModules)
                        {
                            userModulsId += $"{userModule.moduleId};";
                        }

                        HttpContext.Response.Cookies.Append("Token", token);
                        HttpContext.Response.Cookies.Append("UserModuleIds", userModulsId);
                        HttpContext.Response.Cookies.Append("ActiveFirmNumber", model.FirmNumber.ToString());
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Geçersiz Kullanıcı Adı veya Şifre";
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Bir hata oluştu, lütfen daha sonra tekrar deneyin.");
                return View(model);
            }
        }

        public PartialViewResult PartialNavbar()
        {
            return PartialView();
        }
        public PartialViewResult PartialHeader()
        {
            return PartialView();
        }
        public PartialViewResult PartialScript()
        {
            return PartialView();
        }

    }
}

