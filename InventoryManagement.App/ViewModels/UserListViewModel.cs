using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.App.ViewModels
{
    public class UserListViewModel : ViewModelBase
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Name { get  { return FirstName + " " + LastName; } }
        public bool Active { get; set; }

        public override void InitMapping()
        {
            throw new NotImplementedException();
        }
    }
}