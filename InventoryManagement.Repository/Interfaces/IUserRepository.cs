using InventoryManagement.BE.Product;
using InventoryManagement.BE.User;
using System;
using System.Collections.Generic;

namespace InventoryManagement.Repository.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        User GetUserById(int id);
        User GetUserByUsername(string username);
        List<User> GetAllUsers();
        bool SaveOrUpdateUser(User user, DBContext db);
        bool Delete(int id);
    }
}
