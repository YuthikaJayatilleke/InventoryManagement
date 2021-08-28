using InventoryManagement.BE.SecurityPrivilege;
using InventoryManagement.BE.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace InventoryManagement.App.Context
{
    public class UserContext
    {
        public UserContext()
        {

        }

        public User UserInfo { get; set; }
        public string Token { get; set; }
        public SecurityPrivilege SecurityPrivileges { get; set; }
        public Dictionary<string, bool> RolePrivileges { get; set; }

        private readonly Dictionary<string, object> _parameters = new Dictionary<string, object>();

        public void SetParameter(string key, object value)
        {
            _parameters[key] = value;
        }

        public object GetParameter(string key)
        {
            return _parameters[key];
        }

        public IDictionary<string, object> Parameters { get { return _parameters; } }

        public bool HasPrivilege(string privCode)
        {
            if (RolePrivileges.ContainsKey(privCode))
                return RolePrivileges[privCode];

            return false;
        }

    }
}
