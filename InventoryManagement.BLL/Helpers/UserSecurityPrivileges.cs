using InventoryManagement.BE.SecurityPrivilege;
using InventoryManagement.BE.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace InventoryManagement.BLL.Helpers
{
    public class PrivilegeAttribute : Attribute
    {
    }

    public class UserSecurityPrivileges
    {
        [Browsable(false)]
        public long UserId { get; set; }

        public bool IsSuperUser { get; set; }

        [Privilege, Category("Product"), DisplayName("Add")]
        public bool ProductAdd { get; set; }

        [Privilege, Category("Product"), DisplayName("Edit")]
        public bool ProductEdit { get; set; }

        [Privilege, Category("Product"), DisplayName("List")]
        public bool ProductList { get; set; }

        [Privilege, Category("Product"), DisplayName("View")]
        public bool ProductView { get; set; }


        [Privilege, Category("Merchant"), DisplayName("Add")]
        public bool MerchantAdd { get; set; }

        [Privilege, Category("Merchant"), DisplayName("Edit")]
        public bool MerchantEdit { get; set; }

        [Privilege, Category("Merchant"), DisplayName("List")]
        public bool MerchantList { get; set; }

        [Privilege, Category("Merchant"), DisplayName("View")]
        public bool MerchantView { get; set; }



        [Privilege, Category("User"), DisplayName("Add")]
        public bool UserAdd { get; set; }

        [Privilege, Category("User"), DisplayName("Edit")]
        public bool UserEdit { get; set; }

        [Privilege, Category("User"), DisplayName("List")]
        public bool UserList { get; set; }

        [Privilege, Category("User"), DisplayName("View")]
        public bool UserView { get; set; }


        [Privilege, Category("Email"), DisplayName("Add")]
        public bool EmailAdd { get; set; }

        [Privilege, Category("Email"), DisplayName("List")]
        public bool EmailList { get; set; }


        [Privilege, Category("Email"), DisplayName("Send Bulk Email(Send Grid)")]
        public bool EmailBulkEmail { get; set; }


        public void ResetAllPrivileges(bool value)
        {
            PropertyInfo[] props = typeof(UserSecurityPrivileges).GetProperties();
            foreach (PropertyInfo p in props)
            {
                if (!p.IsDefined(typeof(PrivilegeAttribute), false))
                    continue;

                p.SetValue(this, value, null);
            }
        }

        public UserSecurityPrivileges GetPrivileges(User user)
        {
            var secPriv = new UserSecurityPrivileges() { UserId = user.Id, IsSuperUser = user.IsSuperUser };

            PropertyInfo[] props = typeof(UserSecurityPrivileges).GetProperties();
            foreach (PropertyInfo p in props)
            {
                if (!p.IsDefined(typeof(PrivilegeAttribute), false))
                    continue;

                var priv = user.Privileges.FirstOrDefault(x => x.Code == p.Name);
                if (priv == null)
                    continue; //Skip undefined privileges

                p.SetValue(secPriv, priv.Value, null);
            }
            return secPriv;
        }
    }
}
