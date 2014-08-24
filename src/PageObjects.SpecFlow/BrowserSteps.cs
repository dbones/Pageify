using System;
using System.IO;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using TechTalk.SpecFlow;

namespace PageObjects.SpecFlow
{
    [Binding]
    public class BrowserSteps
    {
        private readonly TestContext _ctx;

        public BrowserSteps(TestContext ctx)
        {
            _ctx = ctx;
        }


        [Given("I have (.*) open")]
        [Given("I use (.*)")]
        public void OpenBrowser(string browser)
        {
            //NOTE:this will work with MS-Test, not resharper
            var currentDirectory = Directory.GetCurrentDirectory().ToLower().Replace("\\bin", "").Replace("\\debug", "").Replace("\\release", "");

            IWebDriver driver;
            switch (browser.ToLower())
            {
                case "firefox":
                    driver = new FirefoxDriver();
                    break;
                case "chrome":
                    driver = new ChromeDriver(currentDirectory);
                    break;
                case "ie":
                    driver = new InternetExplorerDriver(currentDirectory, new InternetExplorerOptions() { IntroduceInstabilityByIgnoringProtectedModeSettings = true });
                    break;
                default:
                    throw new NotSupportedException(browser);
            }

            driver.Manage().Window.Maximize();

            _ctx.Driver = driver;
        }


        [Given("I navigate to the (.*) page")]
        public void NavigateToPage(string pageName)
        {
            NavigateToPage(pageName, null);
        }

        [Given("I navigate to the (.*) page at (.*)")]
        public void NavigateToPage(string pageName, string basePage)
        {
            string required = string.Format("{0}Page", pageName).ToLower();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            
            var pages = assemblies
                .Where(asm => !asm.IsDynamic)
                .SelectMany(assembly => assembly.GetExportedTypes())
                .Where(type => typeof(PageBase).IsAssignableFrom(type));

            var p = pages.First(page => page.Name.ToLower() == required);

            var instance = (PageBase)Activator.CreateInstance(p, _ctx.Driver);
            instance.Open(basePage);
            CheckSslWarningPage();


            Action ensurePageIsloaded = () =>
            {
                instance.EnsurePageIsLoaded(basePage);
                _ctx.CurrentPage = instance;
            };


            if (SecurityChallange(ensurePageIsloaded))
            {
                return;
            }

            ensurePageIsloaded();


        }

        private void CheckSslWarningPage()
        {
            var driver = _ctx.Driver;

            //work around for IE, needs to be tested
            var ieOverride = driver.TryFindElement(By.Id("overridelink"));
            if (ieOverride != null)
            {
                ieOverride.Click();
                return;
            }
        }

        private bool SecurityChallange(Action request)
        {
            var login = new LoginPage(_ctx.Driver);
            var application = new Uri(_ctx.Driver.Url).AbsolutePath;
            if (login.Url.OriginalString == application)
            {
                ((dynamic)_ctx).OutstandingRequest = request;
                return true;
            }
            return false;
        }



        [AfterScenario]
        public void TearDown()
        {
            var driver = _ctx.Driver;
            if (driver != null)
            {
                driver.Dispose();
            }
        }
    }
}
