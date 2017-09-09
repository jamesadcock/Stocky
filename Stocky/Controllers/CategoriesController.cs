using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Stocky.Models;
using Stocky.ViewModels;

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

        public ActionResult New()
        {
            var categories = _context.Categories.ToList();
  
            return View("CategoryForm");
        }


        [HttpPost]
        public ActionResult Save(Category category)
        {
            if (category.Id == 0)
            {
                _context.Categories.Add(category);
            }
            else
            {
                var categoryInDb = _context.Categories.Single(c => c.Id == category.Id);
                categoryInDb.Name = category.Name;


            }
            _context.SaveChanges();

            return RedirectToAction("Index", "Categories");
        }

        public ActionResult Edit(int id)
        {
            var category = _context.Categories.SingleOrDefault(c => c.Id == id);

            if (category == null)
                return HttpNotFound();


            return View("CategoryForm", category);

        }


        public ViewResult Index()
        {
            var categories = _context.Categories.ToList();

            return View(categories);
        }

    }
}