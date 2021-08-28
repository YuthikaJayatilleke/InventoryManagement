using InventoryManagement.Repository;
using InventoryManagement.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryManagement.DAL.Product
{
    public class ProductDLL : IProductRepository
    {
        private DBContext _dBContext;

        public ProductDLL(DBContext dBContext)
        {
            _dBContext = dBContext;
        }
        public bool Delete(int id)
        {
            try
            {
                var product = _dBContext.Products.Find(id);
                _dBContext.Remove(product);
                _dBContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dBContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }


        public List<BE.Product.Product> GetAllProducts()
        {
            try
            {
                return _dBContext.Products.ToList();
            }
            catch (Exception)
            {
                return new List<BE.Product.Product>();
            }

        }

        public BE.Product.Product GetProductById(int id)
        {
            try
            {
                return _dBContext.Products.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception)
            {
                return new BE.Product.Product();
            }
        }

        public bool SaveOrUpdateProduct(BE.Product.Product product, DBContext db)
        {
            try
            {
                db.Entry(product).State = product.Id == 0 ? EntityState.Added : EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
