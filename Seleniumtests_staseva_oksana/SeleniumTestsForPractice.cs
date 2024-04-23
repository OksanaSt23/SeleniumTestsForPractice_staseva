using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Size = System.Drawing.Size;

namespace Seleniumtests_staseva_oksana;

public class SeleniumTestsForPractice
{

    public ChromeDriver driver;
    protected WebDriverWait wait;

    [SetUp]
    public void SetUp()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensions");

        driver = new ChromeDriver(options);

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5)); 
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3); 
        
        Autorization();  
    }
    public void Autorization() 
    {
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");

        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("m-b2005@yandex.ru");

        var password = driver.FindElement(By.Name("Password"));
        password.SendKeys("OksanaSt-123");

        var enter = driver.FindElement(By.Name("button"));
        enter.Click();

        wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='Title']")));
    }

    [Test]
    public void Authorization()
    {
        var currentUrl = driver.Url;
        Assert.That(currentUrl == "https://staff-testing.testkontur.ru/news",
            "current url = " + currentUrl + ", а должен быть https://staff-testing.testkontur.ru/news");
    }

    [Test]
    public void NavigationTest()
    {
        driver.Manage().Window.Size = new Size(1280, 1024);  
        var sideMenu = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        sideMenu.Click();
        var community = driver.FindElements(By.CssSelector("[data-tid='Community']"))
            .First(element => element.Displayed);
        community.Click();
        var communityTitle = driver.FindElement(By.CssSelector("[data-tid='Title']"));
        var currentUrl = driver.Url;
        Assert.That(currentUrl == "https://staff-testing.testkontur.ru/communities",
            "current url = " + currentUrl + ", а должен быть https://staff-testing.testkontur.ru/communities");
    }

    [Test]
    public void CommunitiesMember()
    {
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/communities");
        var member = driver.FindElement(By.LinkText("Я участник"));
        member.Click();
        
        var currentUrl = driver.Url;
        Assert.That(currentUrl == "https://staff-testing.testkontur.ru/communities?activeTab=isMember",
            "current url = " + currentUrl + ", а должен быть https://staff-testing.testkontur.ru/communities?activeTab=isMember");
    }
    
    [Test]
    public void EditProfileWorkAddress()
    {
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/profile/settings/edit");
        var address = driver.FindElement(By.CssSelector("[data-tid='Address'] [data-tid='Input']"));
        address.SendKeys(Keys.Control + "a");
        address.SendKeys(Keys.Delete);
        address.SendKeys("Санкт-Петербург");
        var save = driver.FindElement(By.XPath("//button[text()='Сохранить']"));
        save.Click();

        var searchcity = driver
            .FindElement(By.CssSelector("[data-tid='ContactCard']"))
            .FindElement(By.XPath("//*[text()='Санкт-Петербург']"));
        
        var expectedCityText = "Санкт-Петербург";
        var actualCityText = searchcity.Text;
        
        Assert.That(expectedCityText == actualCityText,
            $"current text = {actualCityText}, а должен быть {expectedCityText}");
    }
    
    [Test]
    public void AddCommunities()
    {
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/communities");
        var addcommunities = driver.FindElement(By.XPath("//button[text()='СОЗДАТЬ']"));
        addcommunities.Click();
        
        var communityName = $"Сообщество {Guid.NewGuid()}";
        
        var namecommunities = driver.FindElement(By.CssSelector("[data-tid='Name']"));
        namecommunities.SendKeys(communityName);
        
        var messagecommunities = driver.FindElement(By.CssSelector("[data-tid='Message']"));
        messagecommunities.SendKeys("Hi)");
        var buttoncreate = driver.FindElement(By.CssSelector("[data-tid='CreateButton']"));
        buttoncreate.Click();
        var buttonclose = driver.FindElement(By.XPath("//button[text()='Закрыть']"));
        buttonclose.Click();
        Assert.That(driver.FindElement(By.XPath($"//div[text()='{communityName}']")).Text == communityName);
    }
    
    
    [TearDown]
    public void TearDown()
    {
        driver.Quit();
    }
}