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


        public ViewResult Index()
        {
            var categories = _context.Categories.ToList();

            return View(categories);
        }

    }
}