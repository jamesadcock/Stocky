using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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

        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}