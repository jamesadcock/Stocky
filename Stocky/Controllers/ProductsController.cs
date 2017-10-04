using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Stocky.Models;
using Stocky.ViewModels;
using System.Net.Http;
using System;
using AutoMapper;
using Stocky.Dtos;

namespace Stocky.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;


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
                Categories = categories
   
            };
            return View("ProductForm", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Product product)
        {
            // form validation
            if (!ModelState.IsValid)
            {
                var viewModel = new ProductFormViewModel(product)
                {
                    Categories = _context.Categories.ToList()

                };
                    return View("ProductForm", viewModel);
            }
            // create a new product
            if (product.Id == 0)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:49640/api/");

                    product.Categories = ParseProductCategories(product);
                    var productDto = Mapper.Map<Product, ProductDto>(product);

                    var postTask = client.PostAsJsonAsync<ProductDto>("products", productDto);
                    postTask.Wait();

                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }


                product.Categories = ParseProductCategories(product);
                //  _context.Products.Add(product);
            }
            // edit an existing product 
            else
            {
                var productInDb = _context.Products.Include(p => p.Categories).Single(p => p.Id == product.Id);
               
                productInDb.Name = product.Name;
                productInDb.Sku = product.Sku;
                productInDb.Description = product.Description;
                productInDb.Price = product.Price;
                productInDb.Categories = ParseProductCategories(product);

            }
            _context.SaveChanges();

            return RedirectToAction("Index", "Products");
        }


        public ViewResult Index()
        {
            return View();
        }


        public ActionResult Edit(int id)
        {
            var product = _context.Products.Include(p => p.Categories).SingleOrDefault(p => p.Id == id); 

            if (product == null)
                return HttpNotFound();

            var viewModel = new ProductFormViewModel(product)
            {
                Categories = _context.Categories.ToList()
                
            };

            viewModel.CategoryIds= new List<int>();
            foreach (var category in product.Categories)
            {
                viewModel.CategoryIds.Add(category.Id);
            }

            return View("ProductForm", viewModel);

        }
        

        public Collection<Category> ParseProductCategories(Product product)
        {
            Collection<Category> categories = new Collection<Category>();
            foreach (var categoryId in product.CategoryIds)
            {
                var category = _context.Categories.Single(c => c.Id == categoryId);
                categories.Add(category);
            }

            return categories;

        } 
    }


}