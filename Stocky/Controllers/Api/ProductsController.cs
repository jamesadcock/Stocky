﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using AutoMapper;
using Microsoft.Owin.Security.Provider;
using System.Data.Entity;
using Stocky.Dtos;
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


        public IEnumerable<ProductDto> GetProducts(string category)
        {
            _context.Configuration.ProxyCreationEnabled = false;
            // var products = _context.Products.Include(p => p.Categories).ToList();
            var products = _context.Products.Include(p => p.Categories).Where(p => p.Categories.Any(c => c.Name == category)).ToList();
            var productsDto =  products.Select(Mapper.Map<Product, ProductDto>);
            return productsDto;
        }

        public IHttpActionResult GetProduct(int id)
        {
            var product = _context.Products.SingleOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Product, ProductDto>(product));
        }

        [HttpPost]
        public IHttpActionResult CreateProduct(ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var product = Mapper.Map<ProductDto, Product>(productDto);
            _context.Products.Add(product);
            _context.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + product.Id), product);
        }

        [HttpPut]
        public void UpdateCustomer(int id, ProductDto productDto)
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

            Mapper.Map(productDto, productInDb);

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