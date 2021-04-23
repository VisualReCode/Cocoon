using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using Moq;
using ReCode.Cocoon.Legacy.Auth;
using Xunit;

namespace ReCode.Cocoon.Legacy.Tests.Auth
{
    public class AuthApiHandlerTests
    {
        private readonly AuthApiHandler _sut = new AuthApiHandler();

        private readonly Mock<HttpContextBase> _httpContextBase = new Mock<HttpContextBase>();
        private readonly Mock<HttpResponseBase> _httpResponseBase = new Mock<HttpResponseBase>();
        
        
        public AuthApiHandlerTests()
        {
            _httpContextBase.Setup(x => x.Response).Returns(_httpResponseBase.Object);      
        }
        
        [Fact]
        public void ReturnsNotAuthedWhenIdentityIsNotClaimsPrincipal()
        {
            // Act - Arrange
            _sut.ProcessRequest(_httpContextBase.Object);
            
            // Assert
            _httpResponseBase.VerifySet(x => x.StatusCode = 401);
        }
        
        [Fact]
        public void ReturnsNotAuthedWhenIdentityIsNotAuthenticated()
        {
            // Arrange
            var identity = new Mock<IIdentity>();
            var claimsPrincipal = new ClaimsPrincipal(identity.Object);
            _httpContextBase.Setup(x => x.User).Returns(claimsPrincipal);
                
            // Act
            _sut.ProcessRequest(_httpContextBase.Object);
            
            // Assert
            _httpResponseBase.VerifySet(x => x.StatusCode = 401);
        }
        
        [Fact]
        public void ReturnsThePrincipal()
        {
            // Arrange
            var identity = new Mock<IPrincipal>();
            identity.Setup(x => x.Identity.AuthenticationType).Returns("Custom");

            var claimsPrincipal = new ClaimsPrincipal(identity.Object);
            _httpContextBase.Setup(x => x.User).Returns(claimsPrincipal);
                
            // Act
            _sut.ProcessRequest(_httpContextBase.Object);
            
            // Assert
            _httpResponseBase.VerifySet(x => x.StatusCode = 200);
        }


        [Fact]
        public void HandlerIsReusable()
        {
            Assert.False(_sut.IsReusable);
        }
    }
}