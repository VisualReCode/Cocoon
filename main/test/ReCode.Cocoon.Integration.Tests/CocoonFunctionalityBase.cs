using System.Threading.Tasks;
using Xunit;

namespace ReCode.Cocoon.Integration.Tests
{
    public abstract class CocoonFunctionalityBase
    {
        protected abstract string BaseUrl { get; }
        
        protected CocoonFunctionalityBase()
        {
            
        }
        
        [Fact]
        public abstract Task Pages_Available_In_Modern_App_Should_Serve_Before_Cocoon();

        [Fact]
        public abstract Task Pages_UnAvailable_In_Modern_App_Should_Serve_From_Cocoon();

        [Fact]
        public abstract Task Cocoon_Should_Honor_The_Auth_State_From_The_Legacy_App();

    }
}