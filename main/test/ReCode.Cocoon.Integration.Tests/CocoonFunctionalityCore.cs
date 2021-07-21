using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Playwright;
using Xunit;

namespace ReCode.Cocoon.Integration.Tests
{
    public class CocoonFunctionalityCore : CocoonFunctionalityBase
    {
        protected override string BaseUrl => "http://localhost:5003";

        public override async Task Pages_Available_In_Modern_App_Should_Serve_Before_Cocoon()
        {
            // Arrange
            using var playwright = await Playwright.CreateAsync();

            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });

            // Act
            var page = await browser.NewPageAsync();
            await page.GotoAsync(BaseUrl);
            var result = await page.TextContentAsync("body > header > div > div > div.navbar-header > a");
            
            // Assert
            result.Should().Be("Wingtip Toys 2");
        }

        public override async Task Pages_UnAvailable_In_Modern_App_Should_Serve_From_Cocoon()
        {
            // Arrange
            using var playwright = await Playwright.CreateAsync();

            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });

            // Act
            var page = await browser.NewPageAsync();
            await page.GotoAsync($"{BaseUrl}/about");
            var result = await page.TextContentAsync("#ctl01 > div.navbar.navbar-inverse.navbar-fixed-top > div > div.navbar-header > a");
            
            // Assert
            result.Should().Be("Wingtip Toys");
        }

        public override async Task Cocoon_Should_Honor_The_Auth_State_From_The_Legacy_App()
        {
            // Arrange
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });

            // Act
            var page = await browser.NewPageAsync();
            
            // Login
            await page.GotoAsync($"{BaseUrl}/Account/Login");
            await page.FillAsync("#MainContent_UserName", "admin");
            await page.FillAsync("#MainContent_Password", "Pa$$word");
            await page.ClickAsync("input[type='submit']");

            // Find the nav bar item, can take some time as blazor has to reinit
            var accountLink = await page.WaitForSelectorAsync("#account-link", new PageWaitForSelectorOptions
            {
                Timeout = 3000
            });

            var result = await accountLink.GetAttributeAsync("title");
            
            // Assert
            result.Should().Be("Manage your account");
        }
    }
}