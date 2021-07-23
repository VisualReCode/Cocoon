using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Playwright;
using Xunit;

namespace ReCode.Cocoon.Integration.Tests
{
    public class CocoonFunctionalityCore : CocoonFunctionalityBase
    {
        protected override string BaseUrl => "http://localhost:5003";

        public CocoonFunctionalityCore() : base(true)
        {
        }
        
        public override async Task Pages_Available_In_Modern_App_Should_Serve_Before_Cocoon()
        {
            // Arrange
            using var playwright = await Playwright.CreateAsync();

            await using var browser = await playwright.Chromium.LaunchAsync(BrowserTypeLaunchOptions);

            // Act
            var page = await browser.NewPageAsync();
            await page.GotoAsync(BaseUrl);
            var result = await page.TextContentAsync("body > header > div > div > div.navbar-header > a");
            
            // Assert
            result.Should().Be("Wingtip Toys 2 - MVCCore");
        }
    }
}