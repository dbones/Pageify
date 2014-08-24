using System;
using OpenQA.Selenium;

namespace PageObjects
{
    public abstract class PageBase
    {
        protected readonly IWebDriver _driver;
        protected readonly IWebElement _element;

        protected PageBase(IWebDriver driver)
        {
            _driver = driver;
            _element = driver.FindElement(By.TagName("body"));
        }

        /// <summary>
        /// the Url of the page
        /// </summary>
        public abstract Uri Url { get; }


        public virtual bool IsCurrentPage(string basePage = null)
        {
            return _driver.Url == CreateUrl(basePage).ToString();
        }

        public virtual void EnsurePageIsLoaded(string basePage = null)
        {
            if (IsCurrentPage(basePage))
                return;

            Open(basePage);
        }

        /// <summary>
        /// open the page (navigate to it)
        /// </summary>
        public virtual void Open(string baseUrl = null)
        {
            var url = Url.IsAbsoluteUri ? Url : CreateUrl(baseUrl);
            _driver.Navigate().GoToUrl(url.ToString());
        }

        private Uri CreateUrl(string baseUrl)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                var uri = new Uri(_driver.Url);
                baseUrl = string.Format("{0}://{1}", uri.Scheme, uri.Host);
                if (uri.Port != 80)
                {
                    baseUrl += string.Format(":{0}/", uri.Port);
                }
            }

            return new Uri(new Uri(baseUrl), Url);
        }
    }
}