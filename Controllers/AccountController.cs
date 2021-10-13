using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
                //string baseUrl = "https://localhost:44302";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_configuration["BaseUrl"]);
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);

                string stringData = JsonConvert.SerializeObject(login);
                var contentData = new StringContent(stringData,System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync("/login", contentData).Result;
                string stringJWT = response.Content.ReadAsStringAsync().Result;
                //JWT jwt = JsonConvert.DeserializeObject<JWT>(stringJWT);

                //HttpContext.Session.SetString("JWTtoken", jwt.Token);
                HttpContext.Session.SetString("JWTtoken", stringJWT);
                return RedirectToAction("Index", "Tasks");
                //return RedirectToAction(nameof(Index));
            }
            return View();
        }

        //// GET: Account
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Login.ToListAsync());
        //}

        //// GET: Account/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var login = await _context.Login
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (login == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(login);
        //}

        //// GET: Account/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Account/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Email,Password")] Login login)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(login);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(login);
        //}

        //// GET: Account/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var login = await _context.Login.FindAsync(id);
        //    if (login == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(login);
        //}

        //// POST: Account/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Password")] Login login)
        //{
        //    if (id != login.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(login);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!LoginExists(login.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(login);
        //}

        //// GET: Account/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var login = await _context.Login
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (login == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(login);
        //}

        //// POST: Account/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var login = await _context.Login.FindAsync(id);
        //    _context.Login.Remove(login);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool LoginExists(int id)
        //{
        //    return _context.Login.Any(e => e.Id == id);
        //}
    }
}
