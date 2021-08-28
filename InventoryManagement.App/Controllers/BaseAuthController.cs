using System;
using InventoryManagement.App.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.App.Controllers
{
    [BaseAuthenticationFilter]
    public class BaseAuthController : Controller
    {
    }

    public class PrivilegeAttribute : AuthorizeAttribute
    {
        public string Code { get; set; }
        public PrivilegeAttribute(string code)
        {
            Code = code;
        }
    }
}
