using Microsoft.AspNetCore.Mvc;
using MVCClient.Models;
using MVCClient.ViewModels;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

namespace MVCClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private const string SSOUrl = "https://localhost:44315/";
        private const string ResoruceWebAPIUrl = "https://localhost:44345/";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<(string ErrorMessage, string Token)> GetTokenAssync(UserCredentials userCredentials)
        {
            StringContent Content = new StringContent(JsonSerializer.Serialize(userCredentials),
                Encoding.UTF8, "application/json");
            (string ErrorMessage, string Token) Result = (null, null);
            HttpClient HttpClient = new HttpClient();
            var Response = await HttpClient.PostAsync($"{SSOUrl}login", Content);

            if (Response.IsSuccessStatusCode)
            {
                Result.Token = await Response.Content.ReadAsStringAsync();
            }
            else
            {
                Result.Token = null;
                Result.ErrorMessage = Response.ReasonPhrase;
            }

            return Result;
        }

        public async Task<string> GetDataAsync(string url, string token = null)
        {
            var HttpClient = new HttpClient();

            HttpResponseMessage Response;
            string ResultData = "";

            if (token != null)
            {
                HttpClient.DefaultRequestHeaders.Authorization = new
                    System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
            }

            Response = await HttpClient.GetAsync(url);

            if (Response.IsSuccessStatusCode)
            {
                string Result = await Response.Content.ReadAsStringAsync();
                ResultData = Result;
            }
            else
            {
                ResultData = Response.ReasonPhrase;
            }


            return ResultData;
        }

        public string GetClaimValue(string token, string claimType)
        {
            var Handler = new JwtSecurityTokenHandler();
            var Token = Handler.ReadJwtToken(token);
            return Token.Claims.Where(c => c.Type.Equals(claimType, StringComparison.OrdinalIgnoreCase))
                .Select(c => c.Value).FirstOrDefault();
        }

        public string[] GetRoles(string token)
        {
            var Handler = new JwtSecurityTokenHandler();
            var Token = Handler.ReadJwtToken(token);
            return Token.Claims.Where(c => c.Type.Equals("role", StringComparison.OrdinalIgnoreCase))
                .Select(c => c.Value).ToArray();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IndexViewModel data, string button)
        {
            switch (button)
            {
                case "login":
                    (data.InformationMessage, data.Token) = await GetTokenAssync(data.UserCredential);
                    break;
                case "getdata":
                    data.InformationMessage = await GetDataAsync($"{ResoruceWebAPIUrl}getdata");
                    break;
                case "getadmindata":
                    data.InformationMessage = await GetDataAsync($"{ResoruceWebAPIUrl}getadmindata", data.Token);
                    break;
                case "getaccountantdata":
                    data.InformationMessage = await GetDataAsync($"{ResoruceWebAPIUrl}getaccountantdata", data.Token);
                    break;
                case "getsellerdata":
                    data.InformationMessage = await GetDataAsync($"{ResoruceWebAPIUrl}getsellerdata", data.Token);
                    break;
            }

            if (data.Token != null)
            {
                data.FullUserName = $"{GetClaimValue(data.Token, "firstname")} " +
                    $"{GetClaimValue(data.Token, "lastname")}";
                data.Roles = GetRoles(data.Token);
            }

            return View(data);
        }

        public IActionResult Index()
        {
            return View(new IndexViewModel());
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
    }
}