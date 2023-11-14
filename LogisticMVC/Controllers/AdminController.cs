using LogisticMVC.DTOs.APISettings;
using LogisticMVC.DTOs.Module;
using LogisticMVC.Models.ModuleModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LojistikUygulamasi.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApiSettings _apiSettings;
        private readonly IHttpClientFactory _httpClientFactory;


        public AdminController(ApiSettings apiSettings, IHttpClientFactory httpClientFactory)
        {
            _apiSettings = apiSettings;
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet]
        public async Task<ActionResult> GetModuleData()
        {
            string token = HttpContext.Request.Cookies["Token"];
            string[] moduleIds = HttpContext.Request.Cookies["UserModuleIds"].Split(';');

            using (var client = _httpClientFactory.CreateClient())
            {
                var apiUrl = _apiSettings.ApiUrl;
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var response = await client.GetStringAsync($"{apiUrl}/modules");
                var moduleResponse = JsonConvert.DeserializeObject<List<GetAllModulesDTO.Root>>(response);
                var modules = new Dictionary<GetAllModulesDTO.Root, List<GetAllModulesDTO.Root>>();

                var parentModules = moduleResponse
                    .Where(x => x.parentId == null && moduleIds.Any(a => a == x.id))
                    .OrderBy(x => x.screenOrder)
                    .ToList();

                var childModules = moduleResponse
                    .Where(x => x.parentId != null && moduleIds.Any(a => a == x.id))
                    .OrderBy(x => x.parentId)
                    .ThenBy(x => x.screenOrder)
                    .ToList();

                foreach (var parentModule in parentModules)
                {
                    var relatedChildModules = childModules
                        .Where(x => x.parentId == parentModule.id)
                        .ToList();

                    modules[parentModule] = relatedChildModules;
                }
                ParentAndChildViewModel viewModel = new ParentAndChildViewModel
                {
                    userModels = modules
                };
                return View(viewModel);

            }
        }
        public async Task<IActionResult> Index()
        {
            await GetModuleData();
            return View();
        }
        public PartialViewResult PartialSidebar()
        {
            return PartialView();
        }
        public PartialViewResult PartialNavbar()
        {
            return PartialView();
        }
        public PartialViewResult PartialFooter()
        {
            return PartialView();
        }
    }
}
