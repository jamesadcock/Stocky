using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Stocky.Models;
using Stocky.ViewModels;

namespace Stocky.Controllers
{
    public class CategoriesController : Controller
    {
        // Veiw a table of all categories
        [Route("categories")]
        public ViewResult ViewCategories()
        {
            var categories = new List<Category>
            {
                new Category() {Id = Guid.NewGuid(), Name = "Computer Games"},
                new Category() {Id = Guid.NewGuid(), Name = "Consoles"},
                new Category() {Id = Guid.NewGuid(), Name = "DVDs"},
            };

            var viewModel = new CategoriesViewModel
            {
                Categories = categories
            };

            return View(viewModel);
        }

        // Add a new category
        [Route("categories/addcategory")]
        public ViewResult AddCategory()
        {
            var category = new Category() { Id = Guid.NewGuid(), Name = "Add Category" };
            return View("AddEditCategory", category);
        }

        // Edit an existing category
        [Route("categories/editcategory/{id:guid}")]
        public ViewResult EditCategory(Guid id)
        {
            var category = new Category() { Id = Guid.NewGuid(), Name = "Computer Games" };
            return View("AddEditCategory", category);
        }

    }
}