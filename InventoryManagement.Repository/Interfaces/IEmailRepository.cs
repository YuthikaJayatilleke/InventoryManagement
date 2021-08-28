using InventoryManagement.BE.Email;
using InventoryManagement.BE.Merchant;
using InventoryManagement.BE.Product;
using System;
using System.Collections.Generic;

namespace InventoryManagement.Repository.Interfaces
{
    public interface IEmailRepository : IDisposable
    {
        Email GetEmailById(int id);
        List<Email> GetAllEmails();
        List<Email> GetPendingEmails();
        bool SaveOrUpdateEmail(Email email);
        bool SaveOrUpdateEmail(Email email, DBContext dBContext);
        bool Delete(int id);
    }
}
