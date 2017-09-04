using System;
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

        // Veiw a table of all categories
        [Route("")]
        public ViewResult ViewProducts()
        {
            var products = new List<Product>
            {
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = "Adventure",
                    Price = 10.00,
                    Sku = "000001",
                    CategroiesList = new List<string>() { "games"},
                    Description = "Atari 2600 Clasic"
                },
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = "Zelda",
                    Price = 25.00,
                    Sku = "000002",
                    CategroiesList = new List<string>() { "games"},
                    Description = "Snes clasic"
                }
            };

            var viewModel = new ProductsViewModel()
            {
                Products = products
            };

            return View(viewModel);
        }
    }


}