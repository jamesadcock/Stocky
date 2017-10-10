using System;
using System.Web.Mvc;
using Stocky.Models;
using AutoMapper;
using System.Net.Http;
using Stocky.Dtos;
using Newtonsoft.Json;
using System.Configuration;
using System.Web;

namespace Stocky.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly string apiUri = ConfigurationManager.AppSettings["ApiUri"].ToString();


        // render view for category list
        public ViewResult Index()
        {
            return View();
        }


        // render new category form
        public ActionResult New()
        {
            var category = new Category();
            return View("CategoryForm", category);
        }


        // render edit category form
        public ActionResult Edit(int id)
        {
            using (var client = new HttpClient())
            {
                // get the categories from the API
                HttpResponseMessage response = client.GetAsync(apiUri + "categories/" + id).Result;
                String content = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    Category category = JsonConvert.DeserializeObject<Category>(content);
                    return View("CategoryForm", category);
                }
                else
                {
                    throw new HttpException(500, response.ReasonPhrase);
                }
            }
        }


        // forwards new category data or edited category data to API
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View("CategoryForm", category);
            }

            using (var client = new HttpClient())
            {
                var categoryDto = Mapper.Map<Category, CategoryDto>(category);

                client.BaseAddress = new Uri(apiUri);
                HttpResponseMessage response;

                // create a new category
                if (category.Id == 0)
                {
                    response = client.PostAsJsonAsync<CategoryDto>("categories", categoryDto).Result;
                }
                else //edit an existing category
                {
                    response = client.PutAsJsonAsync<CategoryDto>("categories/" + category.Id, categoryDto).Result;
                }

                // redirect to list of categories if successful
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    throw new HttpException(500, response.ReasonPhrase);
                }
            }     
        }
    }
}