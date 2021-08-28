using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using InventoryManagement.BE;
namespace InventoryManagement.BE.User
{
    [Table("users")]
    public class User : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public bool IsSuperUser { get; set; }
        public IList<SecurityPrivilege.SecurityPrivilege> Privileges { get; set; }

    }
}
