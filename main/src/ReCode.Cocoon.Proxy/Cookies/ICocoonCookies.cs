using System.Threading.Tasks;
using JetBrains.Annotations;

namespace ReCode.Cocoon.Proxy.Cookies
{
    [PublicAPI]
    public interface ICocoonCookies
    {
        ValueTask<string?> GetAsync(string key);
        Task SetAsync(string key, string value);
    }
}