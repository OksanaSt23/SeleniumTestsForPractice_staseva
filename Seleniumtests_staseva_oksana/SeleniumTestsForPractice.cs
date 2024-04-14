using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Seleniumtests_staseva_oksana;

public class SeleniumTestsForPractice
{
    [Test]
    public void Authorization()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensions");
        
        // зайти в хром (с помощью вебдрайвера)
        var driver = new ChromeDriver();
        
        //       перейти по урлу https://staff-testing.testkontur.ru
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
        Thread.Sleep(3000);
        
        // ввести логин и пароль
        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("m-b2005@yandex.ru");

        var password = driver.FindElement(By.Name("Password"));
        password.SendKeys("OksanaSt-123");
        
        Thread.Sleep(2000);
        
        //       нажать на кнопку "войти"
        var enter = driver.FindElement(By.Name("button"));
        enter.Click();
        
        Thread.Sleep(3000);
        
        //             проверяем что мы находимся на нужной странице
        var currentUrl = driver.Url;
        Assert.That(currentUrl == "https://staff-testing.testkontur.ru/news");
        
        // закрываем браузер и убиваем процесс драйвера
        driver.Quit();
    }
}