﻿using AutoMapper;
using Stocky.Dtos;
using Stocky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Stocky.Controllers.Api
{
    public class CategoriesController : ApiController
    {
        private ApplicationDbContext _context;

        public CategoriesController()
        {
            _context = new ApplicationDbContext();
        }


        // get all categroies
        public IEnumerable<CategoryDto> GetCategories()
        {
            var categoriesDto = _context.Categories.ToList().Select(Mapper.Map<Category, CategoryDto>);
            return categoriesDto;
        }


        // get specified category
        public IHttpActionResult GetCategory(int id)
        {
            var category = _context.Categories.SingleOrDefault(p => p.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Category, CategoryDto>(category));
        }


        // create a new category
        [HttpPost]
        public IHttpActionResult CreateCategory(CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var category = Mapper.Map<CategoryDto, Category>(categoryDto);
            _context.Categories.Add(category);
            _context.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + category.Id), category);
        }


        // edit and exisiting category
        [HttpPut]
        public void UpdateCategory(int id, CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var categoryInDb = _context.Categories.SingleOrDefault(p => p.Id == id);

            if (categoryInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            Mapper.Map(categoryDto, categoryInDb);

            _context.SaveChanges();
        }


        [HttpDelete]
        public void DeleteCategory(int id)
        {
            var categoryInDb = _context.Categories.SingleOrDefault(p => p.Id == id);

            if (categoryInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            _context.Categories.Remove(categoryInDb);
            _context.SaveChanges();
        }
    }
}
