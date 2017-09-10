using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Stocky.Models;

namespace Stocky.ViewModels
{
    public class ProductFormViewModel
    {
        public IEnumerable<Category> Categories { get; set; }


        public int? Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(10)]
        public string Sku { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public double? Price { get; set; }

        // public Category Category { get; set; }
        [Display(Name = "Category")]

        [Required]
        public int? CategoryId { get; set; }


        public string Title => Id != 0 ? "Edit Product" : "New Product";


        public ProductFormViewModel()
        {
            Id = 0;
        }

        public ProductFormViewModel(Product product)
        {

            Id = product.Id;
            Name = product.Name;
            Sku = product.Sku;
            Description = product.Description;
            Price = product.Price;
            CategoryId = product.CategoryId;
        }
    }

}

