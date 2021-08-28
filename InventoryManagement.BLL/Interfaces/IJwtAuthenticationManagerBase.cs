using InventoryManagement.BE.User;

namespace InventoryManagement.BLL.Interfaces.BLL.Helpers
{
    public interface IJwtAuthenticationManager
    {
        public string Authenticate(User user);
    }
}