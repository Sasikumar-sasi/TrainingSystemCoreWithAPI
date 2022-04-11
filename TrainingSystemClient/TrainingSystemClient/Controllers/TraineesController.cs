using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TrainingSystemClient.Models;

namespace TrainingSystemClient.Controllers
{
    public class TraineesController : Controller
    {
        IConfiguration _configuration;
        string BaseURL;

        public TraineesController(ILogger<TraineesController> logger, IConfiguration configuration)
        {

            _configuration = configuration;
            BaseURL = _configuration.GetValue<string>("BaseURL");

        }

        // GET: TraineesController
        public async Task<ActionResult> Index()
        {
            List<Trainee> trainees = await GetTrainees();
            return View(trainees);
        }

        // GET: TraineesController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            Trainee trainee = await GetTrainees(id);
            return View(trainee);
        }

        // GET: TraineesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TraineesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Trainee trainee)
        {

            var AccessMail = HttpContext.Session.GetString("Email");
            Trainee receivedTrainee = new Trainee();

            HttpClientHandler clientHandler = new HttpClientHandler();


            var httpClient = new HttpClient(clientHandler);


            StringContent content = new StringContent(JsonConvert.SerializeObject(trainee), Encoding.UTF8, "application/json");

            using (var response = await httpClient.PostAsync(BaseURL + "/api/trainees", content))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                receivedTrainee = JsonConvert.DeserializeObject<Trainee>(apiResponse);
                if (receivedTrainee != null)
                {
                    return RedirectToAction("Login");
                }
            }


            ViewBag.Message = "Your Record not Created!!! Please try again";
            return View();


        }


        // GET: TraineesController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Trainee trainee = await GetTrainees(id);
            return View(trainee);
        }


        // POST: TraineesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Trainee UpdatedTrainee)
        {
            UpdatedTrainee.TraineeID = id;
            var accessToken = HttpContext.Session.GetString("Email");


            HttpClientHandler clientHandler = new HttpClientHandler();

            var httpClient = new HttpClient(clientHandler);
            StringContent contents = new StringContent(JsonConvert.SerializeObject(UpdatedTrainee), Encoding.UTF8, "application/json");

            using (var response = await httpClient.PutAsync(BaseURL + "/api/Trainees/" + id, contents))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();

                if (apiResponse != null)
                    return RedirectToAction("Index");
                else
                    return View();
            }
        }


        // GET: TraineesController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            Trainee trainee = await GetTrainees(id);
            return View(trainee);
        }

        // POST: TraineesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {

            var accessEmail = HttpContext.Session.GetString("Email");
            HttpClientHandler clientHandler = new HttpClientHandler();
            var httpClient = new HttpClient(clientHandler);
            var response = await httpClient.DeleteAsync(BaseURL + "/api/trainees/" + id);
            string apiResponse = await response.Content.ReadAsStringAsync();
            return RedirectToAction(nameof(Index));


        }



        [HttpGet]
        public async Task<List<Trainee>> GetTrainees()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            HttpClient client = new HttpClient(clientHandler);

            string JsonStr = await client.GetStringAsync(BaseURL + "/api/trainees");
            var result = JsonConvert.DeserializeObject<List<Trainee>>(JsonStr);
            return result;
        }

        public async Task<Trainee> GetTrainees(int id)
        {

            var accessEmail = HttpContext.Session.GetString("Email");
            Trainee receivedTrainee = new Trainee();

            HttpClientHandler clientHandler = new HttpClientHandler();

            var httpClient = new HttpClient(clientHandler);

            using (var response = await httpClient.GetAsync(BaseURL + "/api/trainees/" + id))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedTrainee = JsonConvert.DeserializeObject<Trainee>(apiResponse);
                }
                else
                    ViewBag.StatusCode = response.StatusCode;
            }
            return receivedTrainee;
        }
        [HttpGet]
        public IActionResult ViewMapped()
        {
            return View();
        }
        public async Task<IActionResult> Filtered(Trainee mapping)
        {
            ViewBag.Mapping = mapping;
            if (mapping != null)
            {
                List<Trainee> trainees = await GetTrainees();
                var filterd = trainees.Where(a => a.MappedTo.Equals(mapping.MappedTo)).ToList();

                return View(filterd);
            }
            else
            {
                return RedirectToAction("ViewMapped");
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Trainee trainee)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Message = "Inputs are not valid";
                return View();
            }
            List<Trainee> trainees = await GetTrainees();
            var obj = trainees.Where(a => a.EmailId.Equals(trainee.EmailId) && a.Password.Equals(trainee.Password)).FirstOrDefault();
            if (obj != null)
            {
                HttpContext.Session.SetString("Email", obj.EmailId.ToString());
                return RedirectToAction("DashBoard", "Trainees");
            }
            else
            {
                ViewBag.Message = "User not found for given Email and Password";
                ViewBag.User = obj.EmailId;
                return View();
            }
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
