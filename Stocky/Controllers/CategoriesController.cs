using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Stocky.Models;
using Stocky.ViewModels;
using AutoMapper;
using System.Net.Http;
using Stocky.Dtos;

namespace Stocky.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext _context;


        public CategoriesController()
        {
            _context = new ApplicationDbContext();
        }


        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }


        // render view for category list
        public ViewResult Index()
        {
            var categories = _context.Categories.ToList();

            return View(categories);
        }


        // render new categroy form
        public ActionResult New()
        {
            var categories = _context.Categories.ToList();
            var category = new Category();
  
            return View("CategoryForm", category);
        }


        // render edit category form
        public ActionResult Edit(int id)
        {
            var category = _context.Categories.SingleOrDefault(c => c.Id == id);

            if (category == null)
                return HttpNotFound();

            return View("CategoryForm", category);
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
                client.BaseAddress = new Uri("http://localhost:49640/api/");
                var categoryDto = Mapper.Map<Category, CategoryDto>(category);
                HttpResponseMessage result;

                // create a new category
                if (category.Id == 0)
                {
                    var postTask = client.PostAsJsonAsync<CategoryDto>("categories", categoryDto);
                    postTask.Wait();
                    result = postTask.Result;
                }
                else //edit an existing category
                {
                    var putTask = client.PutAsJsonAsync<CategoryDto>("categories/" + category.Id, categoryDto);
                    putTask.Wait();
                    result = putTask.Result;
                }

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    throw new HttpException(500, result.ToString());
                }
            }     
        }
    }
}