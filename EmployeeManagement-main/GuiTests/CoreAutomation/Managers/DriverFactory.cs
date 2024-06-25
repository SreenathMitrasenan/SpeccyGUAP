﻿using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Remote;
using System.Security.Policy;
using TechTalk.SpecFlow;
using System.IO;
using CoreAutomation.Utilities;

namespace CoreAutomation.Managers
{
    [Binding]
    public class DriverFactory
    {
        private IWebDriver driver;
        private readonly ScenarioContext _scenarioContext;

        public DriverFactory(ScenarioContext scenarioContext) => _scenarioContext = scenarioContext;



        public  IWebDriver GetDriver()
        {
           // string driverType = Environment.GetCommandLineArgs().Length > 1 ? Environment.GetCommandLineArgs()[1] : FileSystem.GetCurrentBrowser().ToString();
            
            string driverType = FileSystem.GetCurrentBrowser().ToString();
            if (FileSystem.GetCurrentEnvironmentType().ToLower().Trim().Equals("local"))
            {                
                driver = GetLocalDriver(FileSystem.GetBrowser(driverType.ToLower()));
            }
            else
            {
                driver = GetRemoteDriver(FileSystem.GetBrowser(driverType.ToLower()));
            }
            _scenarioContext.Set(driver, "driver");
            return driver;
        }

     

        private static IWebDriver GetLocalDriver(BrowserType browserType)
        {
            var optionC = new ChromeOptions();
            optionC.AddArgument("--start-maximized");
            var optionE = new EdgeOptions();
            optionE.AddArgument("--start-maximized");
            var optionF = new FirefoxOptions();
            optionF.AddArgument("--start-maximized");
            return browserType switch
            {
                BrowserType.Chrome => new ChromeDriver(optionC),
                BrowserType.Firefox => new FirefoxDriver(optionF),
                BrowserType.Safari => new SafariDriver(),
                BrowserType.Edge => new EdgeDriver(optionE),
                _ => new ChromeDriver(optionC)
            };
        }

        private static IWebDriver GetRemoteDriver(BrowserType browserType)
        {
            Uri uri = new Uri("http://localhost:4444");
            var ChromeOptions = new ChromeOptions();
            ChromeOptions.AddArgument("--start-maximized");
            return browserType switch
            {
                BrowserType.Chrome => new RemoteWebDriver(uri, ChromeOptions),
                BrowserType.Firefox => new RemoteWebDriver(uri, new FirefoxOptions()),
                BrowserType.Safari => new RemoteWebDriver(uri, new SafariOptions()),
                BrowserType.Edge => new RemoteWebDriver(uri, new EdgeOptions()),
                _ => new RemoteWebDriver(uri, new ChromeOptions())
            };
        }

        public enum BrowserType
        {
            Chrome,
            Firefox,
            Safari,
            Edge
        }



    }
}