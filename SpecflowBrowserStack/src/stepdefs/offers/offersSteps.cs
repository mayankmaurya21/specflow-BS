using TechTalk.SpecFlow;
using SpecflowBrowserStack.Drivers;
using OpenQA.Selenium;
using System;
using OpenQA.Selenium.Appium.Android;

namespace SpecflowBrowserStack.Steps
{
	[Binding]
	public class offersSteps
	{
		private readonly MobileDriver _driver;
		private static bool result;

		public offersSteps(MobileDriver driver)
		{
			_driver = driver;

		}
		[Given(@"I navigate to website on mobile\.")]
		public void GivenINavigateToWebsiteOnMobile_()
		{
			_driver.Current.Value.Navigate().GoToUrl("https://bstackdemo.com/");
		}

		[Then(@"I click on Sign-In link")]
		public void ThenIClickOnSign_InLink()
		{
			_driver.Current.Value.FindElement(By.Id("signin")).Click();
			System.Threading.Thread.Sleep(4000);
		}

		[When(@"I type ""(.*)"" in username field")]
		public void WhenITypeInUsernameField(string username)
		{
			_driver.Current.Value.FindElement(By.Id("react-select-2-input")).SendKeys(username + "\n");
		}

		[When(@"I type ""(.*)"" in password field")]
		public void WhenITypeInPasswordField(string password)
		{
			_driver.Current.Value.FindElement(By.Id("react-select-3-input")).SendKeys(password + "\n");
		}

		[Then(@"I press Log-In Button")]
		public void ThenIPressLog_InButton()
		{
			_driver.Current.Value.FindElement(By.ClassName("login_password")).Click();
			_driver.Current.Value.FindElement(By.Id("login-btn")).Click();
		}


		[Then(@"I click on Offers link")]
		public void ThenIClickOnOffersLink()
		{ 
			System.Threading.Thread.Sleep(4000);
            _driver.Current.Value.FindElementById("offers").Click();
			
		}

		[Then(@"I should see Offer elements")]
		public void ThenIShouldSeeOfferElements()
		{
			_driver.Current.Value.Context="NATIVE_APP";
			_driver.Current.Value.FindElementByXPath(".//android.widget.Button[@text='Allow']").Click();
			_driver.Current.Value.Navigate().Refresh();
			String text = _driver.Current.Value.FindElement(By.XPath("//div[@class='p-6 text-2xl tracking-wide text-center text-red-50']")).Text;
			result = FluentAssertions.CustomAssertionAttribute.Equals("We've promotional offers in your location.", text);
			if (result)
			{
				((IJavaScriptExecutor)_driver.Current).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \"Tests function Assertion Passed\"}}");
			}
			else
			{
				((IJavaScriptExecutor)_driver.Current).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \"Tests function Assertion Failed\"}}");
			}
		}
	}
}
