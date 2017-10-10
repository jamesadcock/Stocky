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
                String content = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    categories = JsonConvert.DeserializeObject<Collection<Category>>(content);
                }
                else
                {
                    throw new HttpException();
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
                String productContent = getProductResponse.Content.ReadAsStringAsync().Result;

                if (getProductResponse.IsSuccessStatusCode)
                {
                    product = JsonConvert.DeserializeObject<Product>(productContent);
                }
                else
                {
                    return HttpNotFound();
                }
            }

            var viewModel = new ProductFormViewModel(product)
            {
                Categories = getCategories()
            };

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
                client.BaseAddress = new Uri(apiUri);
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
                HttpResponseMessage getCategoryResponse = client.GetAsync(apiUri + "categories").Result;
                String categoryContent = getCategoryResponse.Content.ReadAsStringAsync().Result;

                if (getCategoryResponse.IsSuccessStatusCode)
                {
                    categories = JsonConvert.DeserializeObject<List<Category>>(categoryContent);
                    return categories;
                }
                else
                {
                    throw new HttpException();
                }
            }
        }
    }
}