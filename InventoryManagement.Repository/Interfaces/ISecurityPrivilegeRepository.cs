using InventoryManagement.BE.Merchant;
using InventoryManagement.BE.Product;
using InventoryManagement.BE.SecurityPrivilege;
using System;
using System.Collections.Generic;

namespace InventoryManagement.Repository.Interfaces
{
    public interface ISecurityPrivilegeRepository : IDisposable
    {
        List<SecurityPrivilege> GetSecurityPrivilegeByUserId(int id);
        bool SaveOrUpdateSecurityPrivilege(SecurityPrivilege securityPrivilege, DBContext db);
        bool Delete(int id);
    }
}
