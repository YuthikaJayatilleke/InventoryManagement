using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace InventoryManagement.BE.SecurityPrivilege
{
    [Table("security_privileges")]
    public class SecurityPrivilege : EntityBase
    {
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User.User User { get; set; }
        public string Code { get; set; }
        public bool Value { get; set; }
    }
}
