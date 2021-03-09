using System.Threading.Tasks;

namespace ReCode.Cocoon.Proxy.Cookies
{
    public interface ICocoonCookies
    {
        ValueTask<string> GetAsync(string key);
        Task SetAsync(string key, string value);
    }
}