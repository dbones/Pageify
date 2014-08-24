using System;
using OpenQA.Selenium;

namespace PageObjects
{
    public static class WebDriverExtensions
    {
        public static IWebElement TryFindElement(this IWebDriver driver, By locator)
        {
            var elements = driver.FindElements(locator);
            if (elements.Count == 1)
            {
                return elements[0];
            }
            if (elements.Count == 0)
            {
                return null;
            }
            throw new Exception("there is more than one element which matches the By locator");
        }
    }
}