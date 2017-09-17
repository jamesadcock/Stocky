using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Stocky.Models;

namespace Stocky.Controllers.API
{
    public class ProductsController : ApiController
    {
        private ApplicationDbContext _context;

        public ProductsController()
        {
            _context = new ApplicationDbContext();
        }


        public IEnumerable<Product> GetProducts()
        {
            _context.Configuration.ProxyCreationEnabled = false;
            return _context.Products.ToList();
        }

        public Product GetProduct(int id)
        {
            var product = _context.Products.SingleOrDefault(p => p.Id == id);

            if (product == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return product;
        }

        [HttpPost]
        public Product CreateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            _context.Products.Add(product);
            _context.SaveChanges();

            return product;
        }

        [HttpPut]
        public void UpdateCustomer(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var productInDb = _context.Products.SingleOrDefault(p => p.Id == id);

            if (productInDb == null)
            {
                throw  new HttpResponseException(HttpStatusCode.NotFound);
            }

            productInDb.Name = product.Name;
            productInDb.Description = product.Description;
            productInDb.Sku = product.Sku;
            productInDb.Price = product.Price;
            productInDb.Categories = product.Categories;

            _context.SaveChanges();
        }

        [HttpDelete]
        public void DeleteProduct(int id)
        {
            var productInDb = _context.Products.SingleOrDefault(p => p.Id == id);

            if (productInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            _context.Products.Remove(productInDb);
            _context.SaveChanges();
        }
    }
}
