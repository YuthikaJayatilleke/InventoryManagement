using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using InventoryManagement.BLL.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagement.App.ViewModels
{
    public class MerchantViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string EMail { get; set; }
    }
}