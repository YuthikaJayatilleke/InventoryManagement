using InventoryManagement.Repository;
using InventoryManagement.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryManagement.DAL.Merchant
{
    public class MerchantDLL : IMerchantRepository
    {
        private DBContext _dBContext;

        public MerchantDLL(DBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public bool Delete(int id)
        {
            try
            {
                var merchant = _dBContext.Merchants.Find(id);
                _dBContext.Remove(merchant);
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

        public List<BE.Merchant.Merchant> GetAllMerchants()
        {
            try
            {
                return _dBContext.Merchants.ToList();
            }
            catch (Exception)
            {
                return new List<BE.Merchant.Merchant>();
            }

        }

        public BE.Merchant.Merchant GetMerchantById(int id)
        {
            try
            {
                return _dBContext.Merchants.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception)
            {
                return new BE.Merchant.Merchant();
            }
        }

        public bool SaveOrUpdateMerchant(BE.Merchant.Merchant merchant, DBContext db)
        {
            try
            {
                db.Entry(merchant).State = merchant.Id == 0 ? EntityState.Added : EntityState.Modified;
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
