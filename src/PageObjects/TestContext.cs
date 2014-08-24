using System.Collections.Generic;
using System.Dynamic;
using OpenQA.Selenium;

namespace PageObjects
{
    public class TestContext : DynamicObject
    {
        private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        public PageBase CurrentPage { get; set; }

        public Control CurrentControl { get; set; }

        public IWebDriver Driver { get; set; }

        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            string key = GetKey(binder.Name);
            return _dictionary.TryGetValue(key, out result);
        }

        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {
            var key = GetKey(binder.Name);
            if (_dictionary.ContainsKey(key))
            {
                _dictionary[key] = value;
            }
            else
            {
                _dictionary.Add(key, value);
            }

            return true;
        }

        private string GetKey(string key)
        {
            return key.ToLower();
        }


    }
}