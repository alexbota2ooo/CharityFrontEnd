using CharityView.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Xml.Linq;

namespace CharityView.Controllers
{
    public class HomeController : Controller
    {
        const string SessionApiUrl = "_ApiUrl";
        const string SessionXApiKey = "_XApiKey";
        const string SessionName = "_Name";
        const string SessionUserId = "_UserId";
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, HttpClient client)
        {
            _logger = logger;
            _configuration = configuration;
            _client = client;
        }

        //It has layout
        public async Task<IActionResult> Index()
        {
            ViewData["VisitorCount"] = await GetVisitorCounts();
            var data = await GetAllCausesIndex(6);
            return View(data);
        }

        public IActionResult About()
        {
            return View();
        }

        //It has layout
        public async Task<IActionResult> Causes()
        {
            var data = await GetAllCauses(0);
            return View(data);
        }

        public IActionResult Charities(int causeId)
        {
            return View();
        }

        public async Task<IActionResult> CharityDetails(int charityId, int? userId)
        {
            var data = await GetCharity(charityId, userId);
            return View(data);
        }

        public async Task<IActionResult> CharityVettingCriteria(int charityId)
        {
            var data = await GetCharityVettingCriteria(charityId);
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> CharitiesFilter(CharitiesFilterRequest requestObj)
        {
            var data = await GetAllCharities(requestObj.CauseId, requestObj.Leadership, requestObj.UserId);
            return PartialView("_CharitiesPartial", data);
        }

        public IActionResult FundMe()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult SignOutNow()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult RedrictToPanel()
        {
            string name = HttpContext.Request.Query["name"].ToString();
            HttpContext.Session.SetString(SessionName, name);
            string userId = HttpContext.Request.Query["userId"].ToString();
            HttpContext.Session.SetString(SessionUserId, userId);

            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private async Task<List<CharitiesViewModel>> GetAllCharities(int causeId, string? leadership, int? userId)
        {
            var data = new List<CharitiesViewModel>();
            var apiKey = _configuration.GetValue<string>("XApiKey");
            var apiUrl = _configuration.GetValue<string>("ApiUrl");

            var uri = string.Format(apiUrl + "api/Charity/GetAllCharities?causeId=" + causeId + "&leadership=" + leadership + "&userId=" + userId);
            _client.DefaultRequestHeaders.Add("XApiKey", apiKey);
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                data = JsonConvert.DeserializeObject<List<CharitiesViewModel>>(await response.Content.ReadAsStringAsync());
            }
            return data;
        }
        private async Task<CharitiesViewModel> GetCharity(int charityId, int? userId)
        {
            var data = new CharitiesViewModel();
            var apiKey = _configuration.GetValue<string>("XApiKey");
            var apiUrl = _configuration.GetValue<string>("ApiUrl");

            var uri = string.Format(apiUrl + "api/Charity/GetCharity?charityId=" + charityId + "&userId=" + userId);
            _client.DefaultRequestHeaders.Add("XApiKey", apiKey);
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                data = JsonConvert.DeserializeObject<CharitiesViewModel>(await response.Content.ReadAsStringAsync());
            }
            return data;
        }
        private async Task<List<CausesViewModel>> GetAllCauses(int count)
        {
            var data = new List<CausesViewModel>();
            var apiKey = _configuration.GetValue<string>("XApiKey");
            var apiUrl = _configuration.GetValue<string>("ApiUrl");

            var key = HttpContext.Session.GetString(SessionXApiKey);
            if (key == null)
            {
                HttpContext.Session.SetString(SessionApiUrl, apiUrl);
                HttpContext.Session.SetString(SessionXApiKey, apiKey);
            }

            var uri = string.Format(apiUrl + "api/Cause/GetAllCauses?count=" + count);
            _client.DefaultRequestHeaders.Add("XApiKey", apiKey);
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                data = JsonConvert.DeserializeObject<List<CausesViewModel>>(await response.Content.ReadAsStringAsync());
            }
            return data;
        }
        private async Task<List<VettingCriteriaViewModel>> GetCharityVettingCriteria(int charityId)
        {
            var data = new List<VettingCriteriaViewModel>();
            var apiKey = _configuration.GetValue<string>("XApiKey");
            var apiUrl = _configuration.GetValue<string>("ApiUrl");

            var uri = string.Format(apiUrl + "api/Charity/GetVettingCriterias?charityId=" + charityId);
            _client.DefaultRequestHeaders.Add("XApiKey", apiKey);
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                data = JsonConvert.DeserializeObject<List<VettingCriteriaViewModel>>(await response.Content.ReadAsStringAsync());
            }
            return data;
        }
        private async Task<int> GetVisitorCounts()
        {
            var data = new VisitorCountsViewModel();
            var apiKey = _configuration.GetValue<string>("XApiKey");
            var apiUrl = _configuration.GetValue<string>("ApiUrl");

            var uri = string.Format(apiUrl + "api/Charity/GetVisitorCounts");
            _client.DefaultRequestHeaders.Add("XApiKey", apiKey);
            _client.DefaultRequestHeaders.Add("Accept", "application/json");

            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                data = JsonConvert.DeserializeObject<VisitorCountsViewModel>(await response.Content.ReadAsStringAsync());
            }
            if (data.NumberOfVisits != null)
                return data.NumberOfVisits.Value;
            else return 0;
        }
        private async Task<List<CausesViewModel>> GetAllCausesIndex(int count)
        {
            var data = new List<CausesViewModel>();
            var apiKey = _configuration.GetValue<string>("XApiKey");
            var apiUrl = _configuration.GetValue<string>("ApiUrl");

            var key = HttpContext.Session.GetString(SessionXApiKey);
            if (key == null)
            {
                HttpContext.Session.SetString(SessionApiUrl, apiUrl);
                HttpContext.Session.SetString(SessionXApiKey, apiKey);
            }

            var uri = string.Format(apiUrl + "api/Cause/GetAllCauses?count=" + count);
            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                data = JsonConvert.DeserializeObject<List<CausesViewModel>>(await response.Content.ReadAsStringAsync());
            }
            return data;
        }
    }
}