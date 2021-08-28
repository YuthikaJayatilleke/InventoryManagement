using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.BE.Merchant
{
    [Table("merchants")]
    public class Merchant : EntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string EMail { get; set; }
    }
}
