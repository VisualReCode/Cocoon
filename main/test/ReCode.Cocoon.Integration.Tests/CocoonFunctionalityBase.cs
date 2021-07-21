using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Playwright;
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
        public virtual async Task Pages_UnAvailable_In_Modern_App_Should_Serve_From_Cocoon()
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
            var result = await page.TextContentAsync("#ctl01 > div.container.body-content > p");

            // Assert
            result.Should().Be("Use this area to provide additional information.");
        }

        [Fact]
        public virtual async Task Cocoon_Should_Honor_The_Auth_State_From_The_Legacy_App()
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