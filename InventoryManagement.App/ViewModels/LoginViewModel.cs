using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.App.ViewModels
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }

    }
}