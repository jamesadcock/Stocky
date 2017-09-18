using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Stocky.Dtos;
using Stocky.Models;

namespace Stocky.App_Start
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Product, ProductDto>();
            Mapper.CreateMap<ProductDto, Product>();
        }
    }
}