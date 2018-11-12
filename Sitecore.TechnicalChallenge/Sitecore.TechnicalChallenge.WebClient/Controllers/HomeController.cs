using Newtonsoft.Json;
using Sitecore.TechnicalChallenge.WebClient.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sitecore.TechnicalChallenge.WebClient.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly string _apiHost = ConfigurationManager.AppSettings["ApiHost"];

        public async Task<ActionResult> Index()
        {
            var username = ((ClaimsPrincipal) System.Web.HttpContext.Current.User).FindFirst(ClaimTypes.Name).Value;

            var apiUrl = $"{_apiHost}/api/Member/GetMember?username={username}";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                    ((ClaimsPrincipal)System.Web.HttpContext.Current.User).Claims.First(xx => xx.Type == "AccessToken").Value);
                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = response.Content.ReadAsStringAsync().Result;
                    var member = JsonConvert.DeserializeObject<MemberModel>(responseBody);

                    return View(member);
                }
            }

            return RedirectToAction("Login","Account");
        }
    }
}