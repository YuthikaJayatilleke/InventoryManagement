using InventoryManagement.BLL.Interfaces.BLL.Helpers;
using InventoryManagement.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryManagement.BLL.Helpers
{
    public class CreateEmailHelper : ICreateEmailHelper
    {
        private IProductRepository _produtRepo;
        public CreateEmailHelper(IProductRepository produtRepo)
        {
            _produtRepo = produtRepo;
        }

        public string GetProductStockReport()
        {
            string mailBody = "<table width ='100%' style ='border:Solid 1px Black;'>";

            var produts = _produtRepo.GetAllProducts().Where(x => x.CurrentQty > 0).ToList();

            mailBody += "<tr>";
            mailBody += "<td stlye='color:blue;'>" + "Product" + "</td>";
            mailBody += "<td stlye='color:blue;'>" + "Remaining Qty" + "</td>";
            mailBody += "</tr>";

            foreach (var produt in produts) //Loop through DataGridView to get rows
            {
                mailBody += "<tr>";
                mailBody += "<td stlye='color:blue;'>" + produt.Code +"-"+ produt.Name+ "</td>";
                mailBody += "<td stlye='color:blue;'>" + produt.CurrentQty.ToString("N2")+ "</td>";
                mailBody += "</tr>";
            }
            mailBody += "</table>";

            return mailBody;
        }
    }
}
