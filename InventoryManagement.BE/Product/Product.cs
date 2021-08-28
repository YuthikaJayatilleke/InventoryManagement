using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.BE.Product
{
    [Table("products")]
    public class Product
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal CurrentQty { get; set; }
    }
}
