using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Stocky.Models;

namespace Stocky.ViewModels
{
    public class ProductFormViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public Product Product { get; set; }
    }
}

