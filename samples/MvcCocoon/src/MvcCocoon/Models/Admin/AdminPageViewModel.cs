using System.Collections.Generic;
using MvcCocoon.Data;

namespace MvcCocoon.Models.Admin
{
    public class AdminPageViewModel
    {
        public AdminPageViewModel(List<Product> products)
        {
            Products = products;
        }

        public List<Product> Products { get; }
    }
}