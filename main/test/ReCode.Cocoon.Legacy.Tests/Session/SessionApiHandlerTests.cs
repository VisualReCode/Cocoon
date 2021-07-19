using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;
using Moq;
using ReCode.Cocoon.Legacy.Session;
using Xunit;

namespace ReCode.Cocoon.Legacy.Tests.Session
{
    public class SessionApiHandlerTests
    {
        private readonly SessionApiHandler _sut = new();

        private readonly Mock<HttpContextBase> _httpContextBase = new();
        private readonly Mock<HttpResponseBase> _httpResponseBase = new();
        private readonly Mock<HttpRequestBase> _httpRequestBase = new();
        private readonly Mock<HttpSessionStateBase> _httpSessionBase = new();
        private readonly HttpCookieCollection _responseCookieCollection = new();
        
        
        public SessionApiHandlerTests()
        {
            _httpContextBase.Setup(x => x.Response).Returns(_httpResponseBase.Object);      
            _httpContextBase.Setup(x => x.Request).Returns(_httpRequestBase.Object);
            _httpContextBase.Setup(x => x.Session).Returns(_httpSessionBase.Object);
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
        public void ReturnsNotFoundIfKeyMissingInSession()
        {
            // Arrange
            _httpRequestBase.Setup(x => x.QueryString).Returns(new NameValueCollection
            {
                {"key", "aMissingKey"}
            });

            _httpRequestBase.Setup(x => x.HttpMethod).Returns("GET");
            
            // Act
            _sut.ProcessRequest(_httpContextBase.Object);
            _httpResponseBase.VerifySet(x => x.StatusCode = 404);
        }
        
        [Fact]
        public void GetsAValueFromSession()
        {
            // Arrange
            const string sessionData = "someData";

            _httpSessionBase.Setup(x => x["testValue"]).Returns(sessionData);
            _httpRequestBase.Setup(x => x.QueryString).Returns(new NameValueCollection
            {
                {"key", "testValue"}
            });

            _httpRequestBase.Setup(x => x.HttpMethod).Returns("GET");
            
            // Act
            _sut.ProcessRequest(_httpContextBase.Object);
            
            // Assert
            var verifyData = Encoding.UTF8.GetBytes(sessionData);
            _httpResponseBase.Verify(x => x.BinaryWrite(verifyData));
            _httpResponseBase.VerifySet(x => x.StatusCode = 200);
        }

        [Fact]
        public void SetsAValueInSession()
        {
            // Arrange
            const string sessionData = "someData";

            _httpSessionBase.Setup(x => x["testValue"]).Returns(sessionData);

            using (var inputStream = new MemoryStream(Encoding.ASCII.GetBytes(sessionData)))
            {
                
                _httpRequestBase.Setup(x => x.QueryString).Returns(new NameValueCollection
                {
                    {"key", "testValue"},
                    {"type", "System.String"}
                });

                _httpRequestBase.Setup(x => x.ContentLength).Returns((int)inputStream.Length);
                _httpRequestBase.Setup(x => x.HttpMethod).Returns("PUT");
                _httpRequestBase.Setup(x => x.GetBufferlessInputStream()).Returns(inputStream);
            
                // Act
                _sut.ProcessRequest(_httpContextBase.Object);
            
                // Assert
                _httpSessionBase.VerifySet(x => x["testValue"] = sessionData);
            }
        }
        
        [Fact]
        public void HandlerIsReusable()
        {
            Assert.True(_sut.IsReusable);
        }
    }
}