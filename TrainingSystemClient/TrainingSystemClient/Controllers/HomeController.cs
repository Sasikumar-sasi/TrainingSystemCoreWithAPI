using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using TrainingSystemClient.Models;

namespace TrainingSystemClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        IConfiguration _configuration;
        string BaseURL;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            BaseURL = _configuration.GetValue<string>("BaseURL");

        }

        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Admin admin)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Inputs are not valid";
                return View();
            }
            List<Admin> admins = await GetAdmin();
            var obj = admins.Where(a => a.EmailId.Equals(admin.EmailId) && a.Password.Equals(admin.Password)).FirstOrDefault();
            if (obj != null)
            {
                HttpContext.Session.SetString("Email", obj.EmailId.ToString());
                return RedirectToAction("DashBoard", "Home");
            }
            else
            {
                ViewBag.Message = "User not found for given Email and Password";
                ViewBag.User = obj.EmailId;
                return View();
            }
        }





        [HttpGet]
        public async Task<List<Admin>> GetAdmin()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            HttpClient client = new HttpClient(clientHandler);

            string JsonStr = await client.GetStringAsync(BaseURL + "/api/admins");
            var result = JsonConvert.DeserializeObject<List<Admin>>(JsonStr);
            return result;
        }

        public IActionResult DashBoard()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}