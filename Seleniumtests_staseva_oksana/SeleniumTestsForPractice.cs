using FluentAssertions;
using FluentAssertions.Primitives;
using Microsoft.VisualBasic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V121.Autofill;
using OpenQA.Selenium.DevTools.V121.CSS;
using OpenQA.Selenium.DevTools.V121.SystemInfo;
using OpenQA.Selenium.Internal.Logging;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Size = System.Drawing.Size;

namespace Seleniumtests_staseva_oksana;

public class SeleniumTestsForPractice
{

    public ChromeDriver driver;

    [SetUp]
    public void SetUp()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensions");

        driver = new ChromeDriver(options);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3); // неявное ожидание
        
        Autorization();      
    }
    public void Autorization() //вынесли авторизацию отдельным методом
    {
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");

        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("m-b2005@yandex.ru");

        var password = driver.FindElement(By.Name("Password"));
        password.SendKeys("OksanaSt-123");

        var enter = driver.FindElement(By.Name("button"));
        enter.Click();
        driver.FindElement(By.CssSelector("[data-tid='Title']")); //вынесла эту строчку сюда, так как она нужна мне в 4 тестах
    }

    [Test]
    public void Authorization()
    {
        var currentUrl = driver.Url;
        currentUrl.Should().Be("https://staff-testing.testkontur.ru/news");
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
        currentUrl.Should().Be("https://staff-testing.testkontur.ru/communities?activeTab=isMember");
    }
    
    [Test]
    public void EditProfileWorkAddress()
    {
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/profile/settings/edit");
        //var address = driver.FindElement(By.ClassName("react-ui-1l5eu5f")); решила попробовать зацепиться через родительские (см.ниже)
        var workaddress = driver.FindElement(By.CssSelector("[data-tid='Address']"));
        var address = workaddress.FindElement(By.CssSelector("[data-tid='Input']"));
        //address.Click();
        //address.Clear(); не работает, хотела предварительно очистить поле для ввода
        //address.SendKeys(Keys.Clear); тоже не работает
        //address.SendKeys(Keys.Delete); тоже не работает
        //address.SendKeys("\b\b\b\b\b\b"); костыль с backspace тоже не работает
        address.SendKeys("Санкт-Петербург");
        var save = driver.FindElement(By.XPath("//button[text()='Сохранить']"));
        save.Click();

        // Проверка, что выбранный элемент отображается на странице
        var searchcity = driver.FindElement(By.XPath("//div[text()='Санкт-Петербург']")); //не подцепиться никак(
        string selectedText = searchcity.GetAttribute("value");
        if (selectedText == "Санкт-Петербург")
        {
            Console.WriteLine("Тест успешно пройден!");
        }
        else
        {
            Console.WriteLine("Тест не пройден! Значение 'Санкт-Петербург' не найдено!");
        }
    }
    
    
    [Test]
    public void AddCommunities()
    {
        //driver.FindElement(By.CssSelector("[data-tid='Title']"));
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/communities");
        var addcommunities = driver.FindElement(By.XPath("//button[text()='СОЗДАТЬ']"));
        addcommunities.Click();
        var namecommunities = driver.FindElement(By.CssSelector("[data-tid='Name']"));
        namecommunities.SendKeys("Сообщество любителей автотестов");
        var messagecommunities = driver.FindElement(By.CssSelector("[data-tid='Message']"));
        messagecommunities.SendKeys("Hi)");
        var buttoncreate = driver.FindElement(By.CssSelector("[data-tid='CreateButton']"));
        buttoncreate.Click();
        var buttonclose = driver.FindElement(By.XPath("//button[text()='Закрыть']"));
        buttonclose.Click();
        Assert.That(driver.FindElement(By.XPath("//div[text()='Сообщество любителей автотестов']")).Text is("Сообщество любителей автотестов"));
         // здесь пыталась проверить, что название Сообщества соответствует тому, что я вводила при его создании
    }
    
    
    [TearDown]
    public void TearDown()
    {
        driver.Quit();
    }
}