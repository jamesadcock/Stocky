using System;
using  System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Stocky.Models;
using Stocky.ViewModels;


namespace Stocky.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext _context;


        public ProductsController()
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
            var viewModel = new ProductFormViewModel()
            {
                Categories = categories,
                Product = new Product()
            };
            return View("ProductForm", viewModel);
        }


        [HttpPost]
        public ActionResult Save(Product product)
        {

            if (!ModelState.IsValid)
            {
                var viewModel = new ProductFormViewModel()
                {
                    Product = product,
                    Categories = _context.Categories.ToList()

                };
                    return View("ProductForm", viewModel);
            }
            if (product.Id == 0)
            {
                _context.Products.Add(product);
            }
            else
            {
                var productInDb = _context.Products.Single(c => c.Id == product.Id);
                productInDb.Name = product.Name;
                productInDb.Sku = product.Sku;
                productInDb.Description = product.Description;
                productInDb.Category = product.Category;
                productInDb.Price = product.Price;

            }
            _context.SaveChanges();

            return RedirectToAction("Index", "Products");
        }


        public ViewResult Index()
        {
            var products = _context.Products.Include(p => p.Category).ToList();

            return View(products);
        }

        public ActionResult Edit(int id)
        {
            var product = _context.Products.SingleOrDefault(p => p.Id == id);

            if (product == null)
                return HttpNotFound();

            var viewModel = new ProductFormViewModel()
            {
                Product = product,
                Categories = _context.Categories.ToList()

            };

            return View("ProductForm", viewModel);

        }
    }


}