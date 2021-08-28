using InventoryManagement.BE.Email;
using InventoryManagement.BE.Merchant;
using InventoryManagement.BE.Product;
using InventoryManagement.BE.SecurityPrivilege;
using InventoryManagement.BE.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryManagement.Repository
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<SecurityPrivilege> SecurityPrivileges { get; set; }
    }
}
