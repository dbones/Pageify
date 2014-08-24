using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace PageObjects
{
    /// <summary>
    /// a SPA
    /// </summary>
    public abstract class SpaPage : PageBase
    {
        private readonly Dictionary<string, Module> _modules = new Dictionary<string, Module>();

        protected SpaPage(IWebDriver driver)
            : base(driver)
        {

        }

        public override void EnsurePageIsLoaded(string basePage = null)
        {
            base.EnsurePageIsLoaded(basePage);
            foreach (var module in _modules.Values)
            {
                module.WaitTillLoaded();
            }
        }


        public virtual void AddModule<T>(string name, Func<IWebElement> element) where T : Module
        {
            //this is not production speed, but should be easy to use.
            var instance = (T)Activator.CreateInstance(typeof(T), new object[] { _driver, element });
            _modules.Add(name, instance);
        }

        public virtual T GetModule<T>(string name) where T : Module
        {
            return (T)_modules[name];
        }
    }
}