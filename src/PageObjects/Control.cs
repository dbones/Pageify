using System;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace PageObjects
{
    /// <summary>
    /// this represents a HTML element on the html page
    /// </summary>
    public abstract class Control
    {
        protected readonly IWebDriver _driver;
        protected readonly Func<IWebElement> _getElement;

        protected Control(IWebDriver driver, string name, Func<IWebElement> getElement)
        {
            Name = name;
            _driver = driver;
            _getElement = getElement;
        }

        public string Name { get; protected set; }

        public virtual bool IsLoaded
        {
            get
            {
                return _getElement() != null;
            }
        }

        public bool IsVisible
        {
            get { return _getElement().Displayed; }
        }

        public void HoverOver()
        {
            new Actions(_driver).MoveToElement(_getElement()).Build().Perform(); //removed the .Click()
        }

        public override bool Equals(object obj)
        {
            var control = obj as Control;
            if (control == null)
            {
                return false;
            }

            if (IsLoaded && control.IsLoaded)
            {
                return _getElement() == control._getElement();
            }

            if (!IsLoaded && !control.IsLoaded)
            {
                return _getElement == control._getElement; //if the lamda is the same?? it may work
            }

            return false;
        }
    }
}
