using System.Linq;
using System.Threading.Tasks;
using BlazorServerCocoon.Data;
using Microsoft.EntityFrameworkCore;
using ReCode.Cocoon.Proxy.Session;

namespace BlazorServerCocoon.Services
{
    public class ShoppingCart
    {
        private CocoonSession _session;
        private WingtipToysContext _db;
        
        public ShoppingCart(CocoonSession session, WingtipToysContext db)
        {
            _session = session;
            _db = db;
        }

        public async Task<int> GetCountAsync()
        {
            var cartId = await _session.GetAsync<string>("CartId");
            var cart = await _db.CartItems
                .Where(item => item.CartId == cartId)
                .ToListAsync();
            var count = cart    
                .Select(item => item.Quantity)
                .Sum();
            return count;
        }
    }
}