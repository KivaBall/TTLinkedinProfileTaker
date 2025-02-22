using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using TTLinkedinProfileTaker.Entities;
using ILogger = Serilog.ILogger;

namespace TTLinkedinProfileTaker.Controllers;

[ApiController]
[Route("api")]
public sealed class LinkedinController(
    ILogger logger)
    : ControllerBase
{
    [HttpPost("profile-photo")]
    public IActionResult GetProfilePhoto(ProfilePhotoRequest request)
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");

        var driver = new RemoteWebDriver(new Uri("http://selenium:4444/wd/hub"), options);
        try
        {
            logger.Information("Setting browser window size to 800x800");

            driver.Manage().Window.Size = new Size(800, 800);

            logger.Information("Navigating to LinkedIn login page");

            driver.Navigate().GoToUrl("https://www.linkedin.com/login");

            logger.Information("Entering username");

            var usernameInput = driver.FindElement(By.Id("username"));
            usernameInput.SendKeys(request.Login);

            logger.Information("Entering password and submitting login form");

            var passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.SendKeys(request.Password);
            passwordInput.Submit();

            logger.Information("Waiting for page load after login submission");

            Thread.Sleep(2000);

            if (driver.Url == "https://www.linkedin.com/checkpoint/lg/login-submit")
            {
                logger.Warning("Login failed due to invalid credentials");

                return NotFound("Invalid login credentials");
            }

            if (driver.Url.StartsWith("https://www.linkedin.com/checkpoint/challenge"))
            {
                logger.Information("Challenge detected, handling challenge");

                HandleChallengePage(driver);
            }

            logger.Information("Performing final clicks to open profile photo view");

            PerformClick(driver, 80, 150, 5000);

            logger.Information("Retrieving profile photo link");

            var photoElement = driver.FindElement(By.ClassName("profile-photo-edit__preview"));
            var photoLink = photoElement.GetAttribute("src") ?? string.Empty;

            logger.Information("Profile photo link retrieved");

            return Ok(photoLink);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "An error occurred: {Ex}", ex.Message);

            return StatusCode(500, "An error occurred: " + ex.Message);
        }
        finally
        {
            logger.Information("Closing driver");

            driver.Quit();
        }
    }

    private void HandleChallengePage(RemoteWebDriver driver)
    {
        logger.Information("Handling challenge page");

        Thread.Sleep(5000);

        while (driver.Url.StartsWith("https://www.linkedin.com/checkpoint/challenge"))
        {
            PerformClick(driver, 300, 300, 2000);
            PerformClick(driver, 400, 420, 2000);
        }
    }

    private void PerformClick(IWebDriver driver, int offsetX, int offsetY, int sleepTime)
    {
        logger.Information("Clicking at offset ({OffsetX}, {OffsetY})", offsetX, offsetY);

        new Actions(driver)
            .MoveByOffset(offsetX, offsetY)
            .Click()
            .Perform();

        Thread.Sleep(sleepTime);
    }
}