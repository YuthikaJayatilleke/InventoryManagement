using InventoryManagement.Repository;
using InventoryManagement.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryManagement.DAL.SecurityPrivilege
{
   public class SecurityPrivilegeDLL : ISecurityPrivilegeRepository
    {
        private DBContext _dBContext;
        public SecurityPrivilegeDLL(DBContext dBContext)
        {
            _dBContext = dBContext;
        }
        public bool Delete(int id)
        {
            try
            {
                var privilege = _dBContext.SecurityPrivileges.Find(id);
                _dBContext.Remove(privilege);
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


        public List<BE.SecurityPrivilege.SecurityPrivilege> GetSecurityPrivilegeByUserId(int id)
        {
            try
            {
                return _dBContext.SecurityPrivileges.Where(x => x.User.Id == id).ToList();
            }
            catch (Exception)
            {
                return new List<BE.SecurityPrivilege.SecurityPrivilege>();
            }
        }

        public bool SaveOrUpdateSecurityPrivilege(BE.SecurityPrivilege.SecurityPrivilege securityPrivilege, DBContext db)
        {
            try
            {
                db.Entry(securityPrivilege).State = securityPrivilege.Id == 0 ? EntityState.Added : EntityState.Modified;
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
