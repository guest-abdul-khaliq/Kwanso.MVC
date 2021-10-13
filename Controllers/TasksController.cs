using Kwanso.MVC.Models;
using Kwanso.MVC.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace Kwanso.MVC.Controllers
{
    public class TasksController : Controller
    {
        private readonly IConfiguration _configuration;

        public TasksController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("list-tasks")]
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWTtoken")))
                    return RedirectToAction("Login","Account");
                List<Tasks> taskList = new List<Tasks>();
                
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_configuration["BaseUrl"]);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("JWTtoken").Replace("\"", ""));
                    using (var response = await httpClient.GetAsync("/list-tasks"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        apiResponse = apiResponse.TrimStart('\"');
                        apiResponse = apiResponse.TrimEnd('\"');
                        apiResponse = apiResponse.Replace("\\", "");
                        taskList = JsonConvert.DeserializeObject<List<Tasks>>(apiResponse);
                        
                    }
                }
                return View(taskList);
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [Route("create-task")]
        public IActionResult Create()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWTtoken")))
                return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        [Route("create-task")]
        public async Task<IActionResult> Create(TaskViewModel task)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_configuration["BaseUrl"]);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("JWTtoken").Replace("\"", ""));

                    HttpResponseMessage response = httpClient.PostAsync("/create-task", new StringContent(JsonConvert.SerializeObject(task), System.Text.Encoding.UTF8, "application/json")).Result;
                    string stringJWT = response.Content.ReadAsStringAsync().Result;
                }


                return RedirectToAction("Index", "Tasks");
            }
            return View();
        }

        [HttpGet]
        [Route("bulk-delete")]
        public async Task<IActionResult> Delete()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWTtoken")))
                    return RedirectToAction("Login", "Account");
                List<Tasks> taskList = new List<Tasks>();

                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_configuration["BaseUrl"]);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("JWTtoken").Replace("\"", ""));
                    using (var response = await httpClient.GetAsync("/list-tasks"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        apiResponse = apiResponse.TrimStart('\"');
                        apiResponse = apiResponse.TrimEnd('\"');
                        apiResponse = apiResponse.Replace("\\", "");
                        taskList = JsonConvert.DeserializeObject<List<Tasks>>(apiResponse);

                    }
                }
                return View(taskList);
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [Route("bulk-delete")]
        public async Task<IActionResult> Delete(string ids)
        {
            using (var httpClient = new HttpClient())
            {
                //httpClient.BaseAddress = new Uri(_configuration["BaseUrl"]);
                //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("JWTtoken").Replace("\"", ""));

                //HttpResponseMessage response123 = httpClient.PostAsync("/create-task", new StringContent(JsonConvert.SerializeObject(ids), System.Text.Encoding.UTF8, "application/json")).Result;
                //string stringJWT = response123.Content.ReadAsStringAsync().Result;

                /////////////////////////////////////////////////////////////////////////

                var client = new RestClient("https://localhost:44302/bulk-delete?ids=" + HttpUtility.UrlEncode(ids));
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "Bearer " + HttpContext.Session.GetString("JWTtoken").Replace("\"", ""));
                var body = @"";
                request.AddParameter("text/plain", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
            }


            return RedirectToAction("Index", "Tasks");
        }


    }
}
