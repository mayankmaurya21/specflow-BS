using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using BrowserStack;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Html5;
using OpenQA.Selenium.Remote;
using TechTalk.SpecFlow;
using TechTalk.SpecRun;

namespace SpecflowBrowserStack.Drivers
{
	public class BrowserSeleniumDriverFactory
	{
        private readonly ConfigurationDriver _configurationDriver;
		private readonly TestRunContext _testRunContext;

        public BrowserSeleniumDriverFactory(ConfigurationDriver configurationDriver, TestRunContext testRunContext)
		{
			_configurationDriver = configurationDriver;
			_testRunContext = testRunContext;
		}
        
        public IWebDriver GetForBrowser(int browserId)
		{
            // sets remote URL
            string username = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME");
            if (username == null || username == "")
            {
                username = _configurationDriver.Username;
            }
            string access_key = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");
            if (access_key == null || access_key == "")
            {
                access_key = _configurationDriver.AccessKey;
            }
            string remoteUrl = "https://";
            if (username != null && access_key != null)
            {
                remoteUrl += username + ":" + access_key + "@";
            }
            remoteUrl += _configurationDriver.SeleniumBaseUrl + "/wd/hub";
            DesiredCapabilities caps = new DesiredCapabilities();
            // Set common capabilities like "browserstack.local", project, name, session
            foreach (var tuple in _configurationDriver.CommonCapabilities)
            {
                if (tuple.Key.ToString() == "name")
                {
                    caps.SetCapability(tuple.Key.ToString(), tuple.Value.ToString() + " " + browserId.ToString());
                }
                else
                {
                    caps.SetCapability(tuple.Key.ToString(), tuple.Value.ToString());
                }
            }
            if (browserId == 0)
            {
                var specificCap = _configurationDriver.Single.ToList<IConfigurationSection>()[browserId];
                foreach (var tuple in specificCap.GetChildren().AsEnumerable())
                {
                    caps.SetCapability(tuple.Key.ToString(), tuple.Value.ToString());
                }
                return new RemoteWebDriver(new Uri(remoteUrl), caps);
            }
            else if (browserId == 1)
            {
                var specificCap = _configurationDriver.Local.ToList<IConfigurationSection>()[browserId - 1];
                foreach (var tuple in specificCap.GetChildren().AsEnumerable())
                {
                    caps.SetCapability(tuple.Key.ToString(), tuple.Value.ToString());
                }
                return new RemoteWebDriver(new Uri(remoteUrl), caps);
            }
           
            else if(browserId > 2)
            {
                // Set session specific capability
                var specificCaps = _configurationDriver.Parallel.ToList<IConfigurationSection>()[browserId - 3];
                foreach (var tuple in specificCaps.GetChildren().AsEnumerable())
                {
                    caps.SetCapability(tuple.Key.ToString(), tuple.Value.ToString());
                }
                return new RemoteWebDriver(new Uri(remoteUrl), caps);
            }

          else { return null; }
            // return null;
        }

        public AndroidDriver<AndroidElement> GetForMobileBrowser(int browserId)
        {
            AppiumOptions capability = new AppiumOptions();
            // sets remote URL
            string username = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME");
            if (username == null || username == "")
            {
                username = _configurationDriver.Username;
            }
            string access_key = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");
            if (access_key == null || access_key == "")
            {
                access_key = _configurationDriver.AccessKey;
            }
            string remoteUrl = "https://";
            if (username != null && access_key != null)
            {
                remoteUrl += username + ":" + access_key + "@";
            }
            remoteUrl += _configurationDriver.SeleniumBaseUrl + "/wd/hub";
            if (browserId == 2)
            {
                // Set session specific capability
                var specificCaps = _configurationDriver.Mobile.ToList<IConfigurationSection>()[browserId-2];
                foreach (var tuple in specificCaps.GetChildren().AsEnumerable())
                {
                    capability.AddAdditionalCapability(tuple.Key.ToString(), tuple.Value.ToString());
                }
                return new AndroidDriver<AndroidElement>(new Uri(remoteUrl), capability);
            }
            else { return null; }

        }
        public Local GetLocal(int browserIndex)
        {
            if (browserIndex < 2)
            {
                var specificCap = _configurationDriver.Local.ToList<IConfigurationSection>()[0];

                foreach (var tuple in specificCap.GetChildren().AsEnumerable())
                {
                    if (tuple.Key.ToString() == "browserstack.local")
                    {
                        Local _local = new Local();
                        List<KeyValuePair<string, string>> bsLocalArgs = new List<KeyValuePair<string, string>>() {
                        new KeyValuePair<string, string>("key", _configurationDriver.AccessKey)
                    };
                        _local.start(bsLocalArgs);
                        return _local;
                    }
                }
                return null;
            }
            else
            {
                return null;
            }
        }
	}

}
