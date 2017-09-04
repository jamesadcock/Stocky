using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stocky.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public List<string> CategroiesList { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }   
    }
}