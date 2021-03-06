﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using AutoMapper;
using System.Data.Entity;
using Stocky.Dtos;
using Stocky.Models;
using System.Collections.ObjectModel;

namespace Stocky.Controllers.API
{
    public class ProductsController : ApiController
    {
        private ApplicationDbContext dBcontext;

        public ProductsController()
        {
            dBcontext = new ApplicationDbContext();
        }


        // returns all products in a category
        public IEnumerable<ProductDto> GetProducts(string category)
        {
            dBcontext.Configuration.ProxyCreationEnabled = false;
            var products = dBcontext.Products.Include(p => p.Categories).Where(p => p.Categories.Any(c => c.Name == category)).ToList();
            var productsDto =  products.Select(Mapper.Map<Product, ProductDto>);
            return productsDto;
        }

        // returns specified product
        public IHttpActionResult GetProduct(int id)
        {
            var product = dBcontext.Products.SingleOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Product, ProductDto>(product));
        }


        // adds a new product
        [HttpPost]
        public IHttpActionResult PostNewProduct(ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var product = Mapper.Map<ProductDto, Product>(productDto);
            product.Categories = new Collection<Category>();

            // attach categroies to dbcontext
            foreach (var categoryDto in productDto.Categories)
            {
                var category = dBcontext.Categories.Single(c => c.Id == categoryDto.Id);
                product.Categories.Add(category);
            }

            dBcontext.Products.Add(product);
            dBcontext.SaveChanges();

            productDto.Id = product.Id;
            return Created(new Uri(Request.RequestUri + "/" + product.Id), productDto);
        }


        // edit an existing produt
        [HttpPut]
        public void UpdateProduct(int id, ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var productInDb = dBcontext.Products.SingleOrDefault(p => p.Id == id);

            if (productInDb == null)
            {
                throw  new HttpResponseException(HttpStatusCode.NotFound);
            }
  
            Mapper.Map(productDto, productInDb);

            productInDb.Categories = new Collection<Category>();

            // attach categroies to dbcontext
            foreach (var categoryDto in productDto.Categories)
            {
                var category = dBcontext.Categories.Single(c => c.Id == categoryDto.Id);
                productInDb.Categories.Add(category);
            }

            dBcontext.SaveChanges();
        }

        [HttpDelete]
        public void DeleteProduct(int id)
        {
            var productInDb = dBcontext.Products.SingleOrDefault(p => p.Id == id);

            if (productInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            dBcontext.Products.Remove(productInDb);
            dBcontext.SaveChanges();
        }
    }
}
