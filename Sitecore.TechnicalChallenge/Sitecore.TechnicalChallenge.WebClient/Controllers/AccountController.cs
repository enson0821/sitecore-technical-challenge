using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Sitecore.TechnicalChallenge.WebClient.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.TechnicalChallenge.WebClient.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _apiHost = ConfigurationManager.AppSettings["ApiHost"];

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = $"{_apiHost}/api/Account/Login";

                var buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));
                var byteContent = new ByteArrayContent(buffer);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = await client.PostAsync(apiUrl, byteContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = response.Content.ReadAsStringAsync().Result;

                        if (responseBody == "false")
                        {
                            ModelState.AddModelError("","Invalid username/password");
                            return View(model);
                        }

                        var token = await GetToken();
                        var options = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTime.UtcNow.AddSeconds(int.Parse(token["expires_in"]))
                        };

                        var claims = new[]
                        {
                            new Claim(ClaimTypes.Name, model.Username),
                            new Claim("AccessToken", $"{token["access_token"]}")
                        };

                        var identity = new ClaimsIdentity(claims, "ApplicationCookie");

                        Request.GetOwinContext().Authentication.SignIn(options,identity);
                    }
                }

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut("ApplicationCookie");

            return RedirectToAction("Login");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = $"{_apiHost}/api/Account/Register";

                var buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));
                var byteContent = new ByteArrayContent(buffer);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = await client.PostAsync(apiUrl, byteContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = response.Content.ReadAsStringAsync().Result;

                        if (responseBody != "1")
                        {
                            ModelState.AddModelError("", "Error occur during registration");
                            return View(model);
                        }
                    }
                }

                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }

        private async Task<Dictionary<string,string>> GetToken()
        {
            var apiUrl = $"{_apiHost}/token";

            var content = new Dictionary<string, string>
            {
                {"grant_type", "password"}
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                var response = await client.PostAsync(apiUrl, new FormUrlEncodedContent(content));
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = response.Content.ReadAsStringAsync().Result;
                    if (responseBody.Contains("access_token"))
                    {
                        return JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content.ReadAsStringAsync().Result);
                    }
                }
            }

            return null;
        }
    }
}