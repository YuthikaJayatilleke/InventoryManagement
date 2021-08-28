using InventoryManagement.Repository;
using InventoryManagement.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryManagement.DAL.User
{
    public class UserDLL : IUserRepository
    {
        private DBContext _dBContext;

        public UserDLL(DBContext dBContext)
        {
            _dBContext = dBContext;
        }
        public bool Delete(int id)
        {
            try
            {
                var user = GetUserById(id);
                foreach (var privilege in user.Privileges)
                {
                    _dBContext.Remove(privilege);
                }

                _dBContext.Remove(user);
                _dBContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
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
        public List<BE.User.User> GetAllUsers()
        {
            try
            {
                return _dBContext.Users.ToList();
            }
            catch (Exception)
            {
                return new List<BE.User.User>();
            }
        }

        public BE.User.User GetUserById(int id)
        {
            try
            {
                var user = _dBContext.Users
                      .Where(s => s.Id == id)
                      .FirstOrDefault<BE.User.User>();

                _dBContext.Entry(user).Collection(s => s.Privileges).Load();
                return user;
            }
            catch (Exception)
            {
                return new BE.User.User();
            }
        }

        public BE.User.User GetUserByUsername(string username)
        {
            try
            {
                var user = _dBContext.Users
                     .Where(s => s.Username == username)
                     .FirstOrDefault<BE.User.User>();

                _dBContext.Entry(user).Collection(s => s.Privileges).Load();
                return user;
            }
            catch (Exception)
            {
                return new BE.User.User();
            }
        }

        public bool SaveOrUpdateUser(BE.User.User user, DBContext db)
        {
            try
            {
                db.Entry(user).State = user.Id == 0 ? EntityState.Added : EntityState.Modified;
                db.SaveChanges();

                foreach(var privilege in user.Privileges)
                {
                    privilege.UserId = user.Id;
                    db.Entry(privilege).State = privilege.Id == 0 ? EntityState.Added : EntityState.Modified;
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
