using System;
using Microsoft.AspNetCore.Mvc;
using Kwanso.MVC.Data;
using Kwanso.MVC.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Kwanso.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly KwansoMVCContext _context;
        private readonly IConfiguration _configuration;

        public AccountController(KwansoMVCContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("JWTtoken")))
                return RedirectToAction("Index","Tasks");
            return View();
        }

        [HttpPost]
        public IActionResult Login([Bind("Email,Password")] Login login)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_configuration["BaseUrl"]);
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);

                string stringData = JsonConvert.SerializeObject(login);
                var contentData = new StringContent(stringData,System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync("/login", contentData).Result;
                string stringJWT = response.Content.ReadAsStringAsync().Result;

                HttpContext.Session.SetString("JWTtoken", stringJWT);
                return RedirectToAction("Index", "Tasks");
            }
            return View();
        }
    }
}
