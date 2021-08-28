using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using InventoryManagement.BE.Merchant;
using InventoryManagement.BE.Product;
using InventoryManagement.BE.User;

namespace InventoryManagement.App.ViewModels
{
     public  class AutoMapperProfiles : Profile
    {
        public  AutoMapperProfiles()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();
            CreateMap<Merchant, MerchantViewModel>().ReverseMap();
        }
    }
}
