using InventoryManagement.BE.Merchant;
using InventoryManagement.BE.Product;
using System;
using System.Collections.Generic;

namespace InventoryManagement.Repository.Interfaces
{
    public interface IMerchantRepository : IDisposable
    {
        Merchant GetMerchantById(int id);
        List<Merchant> GetAllMerchants();
        bool SaveOrUpdateMerchant(Merchant merchant, DBContext db);
        bool Delete(int id);
    }
}
