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
using System.Web;

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
        public ActionResult Save(Product product, List<int> categoryIds)
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

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49640/api/");
                product.Categories = ParseProductCategories(categoryIds);
                var productDto = Mapper.Map<Product, ProductDto>(product);
                HttpResponseMessage result;

                // create a new product
                if (product.Id == 0)
                {
                    var postTask = client.PostAsJsonAsync<ProductDto>("products", productDto);
                    postTask.Wait();
                    result = postTask.Result;
                }
                else //edit an existing product
                {
                    var putTask = client.PutAsJsonAsync<ProductDto>("products/" + product.Id, productDto);
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
        

        public Collection<Category> ParseProductCategories(List<int> categoryIds)
        {
            Collection<Category> categories = new Collection<Category>();
            foreach (var categoryId in categoryIds)
            {
                var category = _context.Categories.Single(c => c.Id == categoryId);
                categories.Add(category);
            }

            return categories;

        } 
    }


}