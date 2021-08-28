using InventoryManagement.BE.Product;
using System;
using System.Collections.Generic;

namespace InventoryManagement.Repository.Interfaces
{
    public interface IProductRepository : IDisposable
    {
        Product GetProductById(int id);
        List<Product> GetAllProducts();
        bool SaveOrUpdateProduct(Product product, DBContext db);
        bool Delete(int id);
    }
}
