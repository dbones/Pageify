using System;
using OpenQA.Selenium;

namespace PageObjects
{
    public static class ElementExtensions
    {
        public static IWebElement TryFindElement(this IWebElement element, By locator)
        {
            var elements = element.FindElements(locator);
            if (elements.Count == 1)
            {
                return elements[0];
            }
            if (elements.Count > 0)
            {
                return null;
            }
            throw new Exception("there is more than one element which matches the By locator");
        }
    }
}