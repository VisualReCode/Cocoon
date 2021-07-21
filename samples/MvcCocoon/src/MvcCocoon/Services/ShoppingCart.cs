using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcCocoon.Data;
using ReCode.Cocoon.Proxy.Session;

namespace MvcCocoon.Services
{
    public class ShoppingCart
    {
        private CocoonSession _session;
        
        public ShoppingCart(CocoonSession session)
        {
            _session = session;
        }

        public async Task<int> GetCountAsync()
        {
            var cartId = await _session.GetAsync<string>("CartId");
            return 0;
        }
    }
}