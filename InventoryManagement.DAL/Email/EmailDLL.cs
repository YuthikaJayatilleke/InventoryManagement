using InventoryManagement.Repository;
using InventoryManagement.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryManagement.DAL.Email
{
    public class EmailDLL : IEmailRepository
    {
        private DBContext _dBContext;

        public EmailDLL(DBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public bool Delete(int id)
        {
            try
            {
                var email = _dBContext.Emails.Find(id);
                _dBContext.Remove(email);
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

        public List<BE.Email.Email> GetAllEmails()
        {
            try
            {
                return _dBContext.Emails.ToList();
            }
            catch (Exception)
            {
                return new List<BE.Email.Email>();
            }

        }

        public List<BE.Email.Email> GetPendingEmails()
        {
            try
            {
                return _dBContext.Emails.Where(x => x.Status == BE.Email.EmailStatus.Pending).ToList();
            }
            catch (Exception)
            {
                return new List<BE.Email.Email>();
            }

        }

        public BE.Email.Email GetEmailById(int id)
        {
            try
            {
                return _dBContext.Emails.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception)
            {
                return new BE.Email.Email();
            }
        }

        public bool SaveOrUpdateEmail(BE.Email.Email email)
        {
            try
            {
                _dBContext.Entry(email).State = email.Id == 0 ? EntityState.Added : EntityState.Modified;
                _dBContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool SaveOrUpdateEmail(BE.Email.Email email, DBContext dBContext)
        {
            try
            {
                dBContext.Entry(email).State = email.Id == 0 ? EntityState.Added : EntityState.Modified;
                dBContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
