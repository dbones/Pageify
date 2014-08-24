using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PageObjects
{
    /// <summary>
    /// represents a button on the page
    /// </summary>
    /// <remarks>
    /// http://luizfar.wordpress.com/2010/09/29/page-objects/
    /// </remarks>
    public class Button : Control
    {
        private Func<bool> _hasCompleted;

        public Button(IWebDriver driver, string name, Func<IWebElement> getElement)
            : base(driver, name, getElement)
        {
        }

        public string Text { get { return _getElement().Text; } }

        public void SetWait(Func<bool> hasCompleted)
        {
            _hasCompleted = hasCompleted;
        }

        public virtual void Click()
        {
            _getElement().Click();
            Thread.Sleep(350);


            if (_hasCompleted != null)
            {
                var sucess = new WebDriverWait(_driver, new TimeSpan(0, 1, 0)).Until(drv => _hasCompleted());

                if (!sucess)
                {
                    throw new Exception("Click timed out");
                }
            }
        }

    }
}