using System;
using OpenQA.Selenium;

namespace PageObjects.SpecFlow
{
    public class LoginPage : PageBase
    {
        public LoginPage(IWebDriver driver)
            : base(driver)
        {

            UserNameTextBox = new TextBox(_driver, "UserName",
                                          () => _element.FindElement(By.Id("ctl00_ContentPlaceHolder1_UsernameTextBox")));
            PasswordTextBox = new TextBox(_driver, "Password",
                                          () => _element.FindElement(By.Id("ctl00_ContentPlaceHolder1_PasswordTextBox")));
            SignInButton = new Button(_driver, "SignIn",
                                      () => _element.FindElement(By.Id("ctl00_ContentPlaceHolder1_SubmitButton")));

        }

        public override Uri Url
        {
            get { return new Uri("/adfs/ls/", UriKind.Relative); }
        }

        public TextBox UserNameTextBox { get; private set; }
        public TextBox PasswordTextBox { get; private set; }
        public Button SignInButton { get; private set; }


        public string ErrorMessage { get { return _element.FindElement(By.Id("ctl00_ContentPlaceHolder1_ErrorTextLabel")).Text; } }
    }
}