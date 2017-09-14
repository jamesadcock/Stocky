using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Stocky.Controllers;

namespace Stocky.Models
{
    public class Product
    {
        public int Id { get; set; }

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
        public double Price { get; set; }

        public List<Category> Categories { get; set; }

        public List<int> CategoryIds { get; set; }
    }
}