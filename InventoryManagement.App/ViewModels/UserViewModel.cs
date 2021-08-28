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
    public class UserViewModel 
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Token { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public bool IsSetPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsSuperUser { get; set; }
        public List<UserPrivilegeViewModel> Privileges { get; set; }
        public void Init(UserSecurityPrivileges userSecurityPrivileges)
        {
            Privileges = new List<UserPrivilegeViewModel>();

            PropertyInfo[] props = typeof(UserSecurityPrivileges).GetProperties();
            foreach (PropertyInfo p in props)
            {
                if (!p.IsDefined(typeof(PrivilegeAttribute), false))
                    continue;

                var categoryAttr = (CategoryAttribute)p.GetCustomAttributes(typeof(CategoryAttribute), false).First();
                var displayNameAttr = (DisplayNameAttribute)p.GetCustomAttributes(typeof(DisplayNameAttribute), false).First();
                var value = (bool)p.GetValue(userSecurityPrivileges, null);

                var priv = new UserPrivilegeViewModel(p.Name, displayNameAttr.DisplayName, value, categoryAttr.Category);
                Privileges.Add(priv);
            }
        }
    }

    public class UserPrivilegeViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public bool IsSelected { get; set; }

        public UserPrivilegeViewModel(string code, string name, bool isSelected, string category)
        {
            Code = code;
            Name = name;
            IsSelected = isSelected;
            Category = category;
        }
    }
}