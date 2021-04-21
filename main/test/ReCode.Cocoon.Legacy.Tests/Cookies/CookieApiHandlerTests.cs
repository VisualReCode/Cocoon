using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;
using Moq;
using ReCode.Cocoon.Legacy.Cookies;
using Xunit;

namespace ReCode.Cocoon.Legacy.Tests.Cookies
{
    public class CookieApiHandlerTests
    {

        private readonly CookieApiHandler _sut = new CookieApiHandler();

        private readonly Mock<HttpContextBase> _httpContextBase = new Mock<HttpContextBase>();
        private readonly Mock<HttpResponseBase> _httpResponseBase = new Mock<HttpResponseBase>();
        private readonly Mock<HttpRequestBase> _httpRequestBase = new Mock<HttpRequestBase>();
        private readonly HttpCookieCollection _responseCookieCollection = new HttpCookieCollection();
        
        public CookieApiHandlerTests()
        {
            _httpContextBase.Setup(x => x.Response).Returns(_httpResponseBase.Object);      
            _httpContextBase.Setup(x => x.Request).Returns(_httpRequestBase.Object);
            _httpResponseBase.Setup(x => x.Cookies).Returns(_responseCookieCollection);
        }
        
        [Fact]
        public void AMissingKeyResultsInABadRequest()
        {
            // Arrange
            _httpContextBase.Setup(x => x.Request.QueryString).Returns(new NameValueCollection());
            
            // Act
            _sut.ProcessRequest(_httpContextBase.Object);
            
            // Assert
            _httpResponseBase.VerifySet(x => x.StatusCode = 400);
        }

        [Fact]
        public void GetsAValueFromACookie()
        {
            // Arrange
            const string cookieData = "someData";
            
            _httpRequestBase.Setup(x => x.QueryString).Returns(new NameValueCollection
            {
                {"key", "testValue"}
            });
            _httpRequestBase.Setup(x => x.HttpMethod).Returns("GET");
            _httpRequestBase.Setup(x => x.Cookies).Returns(new HttpCookieCollection
            {
                new HttpCookie("testValue", cookieData)
            });
            
            // Act
            _sut.ProcessRequest(_httpContextBase.Object);
            
            var verifyData = Encoding.UTF8.GetBytes(cookieData);
            _httpResponseBase.Verify(x => x.BinaryWrite(verifyData));
            _httpResponseBase.VerifySet(x => x.StatusCode = 200);
        }

        [Fact]
        public void AMissingCookieReturnsNotFound()
        {
            // Arrange
            _httpRequestBase.Setup(x => x.QueryString).Returns(new NameValueCollection
            {
                {"key", "testValue"}
            });
            _httpRequestBase.Setup(x => x.HttpMethod).Returns("GET");
            _httpRequestBase.Setup(x => x.Cookies).Returns(new HttpCookieCollection());
            
            // Act
            _sut.ProcessRequest(_httpContextBase.Object);
            _httpResponseBase.VerifySet(x => x.StatusCode = 404);
        }

        [Fact]
        public void SetACookieValue()
        {
            // Arrange
            _httpRequestBase.Setup(x => x.QueryString).Returns(new NameValueCollection
            {
                {"key", "testValue"}
            });
            
            _httpRequestBase.Setup(x => x.HttpMethod).Returns("PUT");
            _httpRequestBase.Setup(x => x.Cookies).Returns(new HttpCookieCollection());
            _httpRequestBase.Setup(x => x.GetBufferedInputStream()).Returns(new MemoryStream());
            
            // Act
            _sut.ProcessRequest(_httpContextBase.Object);
            _httpResponseBase.VerifySet(x => x.StatusCode = 200);
            Assert.True(_responseCookieCollection.Count == 1);
        }
        
        [Fact]
        public void HandlerIsReusable()
        {
            Assert.True(_sut.IsReusable);
        }
    }
}