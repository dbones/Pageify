using System;
using OpenQA.Selenium;

namespace PageObjects
{
    /// <summary>
    /// A SPA module
    /// </summary>
    public abstract class Module : Control
    {
        protected Module(IWebDriver driver, string name, Func<IWebElement> getElement)
            : base(driver, name, getElement)
        {
        }
    }
}