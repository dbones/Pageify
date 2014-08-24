using System;
using OpenQA.Selenium;

namespace PageObjects
{
    public class TextBox : Control
    {
        public TextBox(IWebDriver driver, string name, Func<IWebElement> getElement)
            : base(driver, name, getElement)
        {

        }

        public string Text
        {
            get { return _getElement().Text; }
            set { _getElement().SendKeys(value); }
        }
    }
}