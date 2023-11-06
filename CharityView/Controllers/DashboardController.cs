using Microsoft.AspNetCore.Mvc;
using System;

namespace CharityView.Controllers
{
    public class DashboardController : Controller
    {
        const string SessionApiUrl = "_ApiUrl";
        const string SessionXApiKey = "_XApiKey";
        const string SessionName = "_Name";
        const string SessionUserId = "_UserId";
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;

        public DashboardController(ILogger<HomeController> logger, IConfiguration configuration, HttpClient client)
        {
            _logger = logger;
            _configuration = configuration;
            _client = client;
        }

        public IActionResult Index()
        {
            ViewData["Dashboard"] = "active";
            ViewData["Donation-history"] = "";
            ViewData["Favourite-charities"] = "";

            var name = HttpContext.Session.GetString(SessionName);
            if (name == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult DonationHistory()
        {
            ViewData["Dashboard"] = "";
            ViewData["Donation-history"] = "active";
            ViewData["Favourite-charities"] = "";

            var name = HttpContext.Session.GetString(SessionName);
            if (name == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult FavoriteCharities()
        {
            ViewData["Dashboard"] = "";
            ViewData["Donation-history"] = "";
            ViewData["Favourite-charities"] = "active";

            var name = HttpContext.Session.GetString(SessionName);
            if (name == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
