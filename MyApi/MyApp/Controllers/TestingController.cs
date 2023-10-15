using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MyApp.Controllers
{
    public class TestingController : Controller
    {
        private string localUrl = "https://localhost:44302";
        public IActionResult Index()
        {
            List<Customer> data = new List<Customer>();
            try
            {
                using(HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(localUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync("/Customer/GetAllCustomers").Result;
                    client.Dispose();
                    if(response.IsSuccessStatusCode)
                    {
                        string stringData=response.Content.ReadAsStringAsync().Result;
                        data = JsonConvert.DeserializeObject<List<Customer>>(stringData);
                    }
                    else
                    {
                        TempData["error"] = $"{response.ReasonPhrase}";
                    }
                }
            }
            catch(Exception ex)
            {
                TempData["expection"]=ex.Message;
            }
            return View(data);
        }
        public IActionResult AddCustomer()
        {
            Customer cust = new Customer();
            return View(cust);
        }
        [HttpPost]
        public IActionResult AddCustomer(Customer model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress=new Uri(localUrl);
                        var data=JsonConvert.SerializeObject(model);
                        var contentData=new StringContent(data,Encoding.UTF8,"application/json");
                        if(model.Id==0)
                        {
                            HttpResponseMessage response = client.PostAsync("/Customer/AddCustomer", contentData).Result;
                            if(response.IsSuccessStatusCode)
                            {
                                TempData["success"]=response.Content.ReadAsStringAsync().Result;
                            }
                            else
                            {
                                TempData["error"] = $"{response.ReasonPhrase}";
                            }
                        }
                        else
                        {
                            HttpResponseMessage response = client.PutAsync("/Customer/UpdateCustomer",contentData).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                TempData["success"] = response.Content.ReadAsStringAsync().Result;
                            }
                            else
                            {
                                TempData["error"] = $"{response.ReasonPhrase}";
                            }
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "ModelState is not valid");
                    return View(model);
                }
            }
            catch(Exception)
            {
                throw;
            }
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            using(HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(localUrl);
                HttpResponseMessage response=client.DeleteAsync("/Customer/DeleteCustomer/"+id).Result; 
                if(response.IsSuccessStatusCode)
                {
                    TempData["success"]=response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    TempData["error"] = $"{response.ReasonPhrase}";
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Customer cust=new Customer();
            using(HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(localUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("/Customer/GetCustomerById/" + id).Result;
                if(response.IsSuccessStatusCode )
                {
                    string stringData=response.Content.ReadAsStringAsync().Result;
                    cust=System.Text.Json.JsonSerializer.Deserialize<Customer>(stringData, new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive=true
                    });

                }
                else
                {
                    TempData["error"] = $"{response.ReasonPhrase}";
                }
            }
           return View("AddCustomer",cust);
        }
    }
}
