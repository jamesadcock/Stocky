using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Mvc;
using Stocky.Models;
using Stocky.ViewModels;
using System.Net.Http;
using System;
using AutoMapper;
using Stocky.Dtos;
using System.Web;
using System.Configuration;
using Newtonsoft.Json;

namespace Stocky.Controllers
{
    public class ProductsController : Controller
    {
        private readonly string apiUri = ConfigurationManager.AppSettings["ApiUri"].ToString();


        // renders all products view
        public ViewResult Index()
        {
            return View();
        }


        // renders new products form
        public ActionResult New()
        {
            Collection<Category> categories;

            using (var client = new HttpClient())
            {
                // get the categories from the API
                HttpResponseMessage response = client.GetAsync(apiUri + "categories").Result;
               
                if (response.IsSuccessStatusCode)
                {
                    String content = response.Content.ReadAsStringAsync().Result;
                    categories = JsonConvert.DeserializeObject<Collection<Category>>(content);
                }
                else
                {
                    throw new HttpException(500, response.ReasonPhrase);
                }
            }

            var viewModel = new ProductFormViewModel()
            {
                Categories = categories
            };
            return View("ProductForm", viewModel);
        }


        // renders edit product form
        public ActionResult Edit(int id)
        {
            Product product;
            using (var client = new HttpClient())
            {
                // get product with specifed id
                HttpResponseMessage getProductResponse = client.GetAsync(apiUri + "products/" + id).Result;

                if (getProductResponse.IsSuccessStatusCode)
                {
                    String content = getProductResponse.Content.ReadAsStringAsync().Result;
                    product = JsonConvert.DeserializeObject<Product>(content);
                }
                else
                {
                    return HttpNotFound();
                }
            }

            // get full list of products
            var viewModel = new ProductFormViewModel(product)
            {
                Categories = getCategories()
            };

            // list of categry id for multi-select
            viewModel.CategoryIds = new List<int>();

            foreach (var category in product.Categories)
            {
                viewModel.CategoryIds.Add(category.Id);
            }

            return View("ProductForm", viewModel);
        }


        // forwards new product data or edited product data to API
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Product product, List<int> categoryIds)
        {
            // form validation
            if (!ModelState.IsValid)
            {
                var viewModel = new ProductFormViewModel(product)
                {
                    Categories = getCategories()

                };
                return View("ProductForm", viewModel);
            }

            using (var client = new HttpClient())
            {
                product.Categories = ParseProductCategories(categoryIds);
                var productDto = Mapper.Map<Product, ProductDto>(product);

                client.BaseAddress = new Uri(apiUri);
                HttpResponseMessage response;

                // create a new product
                if (product.Id == 0)
                {
                    response = client.PostAsJsonAsync<ProductDto>("products", productDto).Result;
                }
                else //edit an existing product
                {
                    response = client.PutAsJsonAsync<ProductDto>("products/" + product.Id, productDto).Result;
                }


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


        // transforms list category Id's to list of category objects
        public Collection<Category> ParseProductCategories(List<int> categoryIds)
        {
            Collection<Category> categories = new Collection<Category>();
            foreach (var categoryId in categoryIds)
            {
               foreach(Category category in getCategories())
                {
                    if (category.Id == categoryId)
                        categories.Add(category);
                }
            }
            return categories;
        } 


        public List<Category> getCategories()
        {
            using (var client = new HttpClient())
            {
                List<Category> categories;
                // get categroies for category list
                HttpResponseMessage response = client.GetAsync(apiUri + "categories").Result;

                if (response.IsSuccessStatusCode)
                {
                    String content = response.Content.ReadAsStringAsync().Result;
                    categories = JsonConvert.DeserializeObject<List<Category>>(content);
                    return categories;
                }
                else
                {
                    throw new HttpException(500, response.ReasonPhrase);
                }
            }
        }
    }
}