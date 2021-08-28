using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.App.ViewModels
{
    public class MerchantListViewModel : ViewModelBase
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
      
        public override void InitMapping()
        {
            throw new NotImplementedException();
        }
    }
}