using AutoMapper;
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


        public IEnumerable<CategoryDto> GetCategories()
        {
            var categoriesDto = _context.Categories.ToList().Select(Mapper.Map<Category, CategoryDto>);
            return categoriesDto;
        }
    }
}
