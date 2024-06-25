using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CoreAutomation.Managers.DriverFactory;

namespace CoreAutomation.Utilities
{
    public class FileSystem
    {
        public static string GetApplicationRootDir()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
        public static string GetCurrentWorkingDir()
        {
            return Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        }
        public static JObject ReadJsonFile(string jsonfilePath)
        {
            return Newtonsoft.Json.Linq.JObject.Parse(File.ReadAllText(jsonfilePath));
        }

        public static JObject ReadApiRequestFile(string jsonfileName)
        {
            var iRequest= GetApiRequestFile(jsonfileName);
            return Newtonsoft.Json.Linq.JObject.Parse(File.ReadAllText(iRequest));
        }

        public static string GetApiRequestFile(string jsonfileName)
        {
            if (string.IsNullOrWhiteSpace(jsonfileName))
                throw new ArgumentException("File name cannot be null or whitespace.", nameof(jsonfileName));

            // Ensure the filename ends with .json
            string fileNameWithExtension = jsonfileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase)
                                           ? jsonfileName
                                           : jsonfileName + ".json";
            // Get the full path to the file
            string iRequestFile = Path.Combine(GetApiRequestFilePath(), fileNameWithExtension);
            // Check if the file exists
            if (!File.Exists(iRequestFile))
                throw new FileNotFoundException($"The file '{iRequestFile}' was not found.");

            return iRequestFile;


        }

        public static string GetConfigFilePath()
        {
            return Path.Combine(GetConfigurationDir(), "Config.json");
        }

        public static string GetConfigurationDir()
        {
            return Path.Combine(GetCurrentWorkingDir(), "Configuration");
        }
        public Dictionary <string,string> MapJsonToDictionary(string jsontextfilepath, string token)
        {
            return JObject.Parse(File.ReadAllText(jsontextfilepath)).SelectToken(token).ToObject<Dictionary<string,string>>();
        }
        public static string GetEMAppUrl()
        {
            return ReadJsonFile(GetConfigFilePath()).SelectToken("url").SelectToken("QAT").ToString().Trim();
        }

        public static string GetSwagAppUrl()
        {
            return ReadJsonFile(GetConfigFilePath()).SelectToken("url").SelectToken("swagapp").ToString().Trim();
        }

        public static string GetCurrentBrowser()
        {
            return ReadJsonFile(GetConfigFilePath()).SelectToken("browserinfo").SelectToken("currentbrowser").ToString().Trim();
        }
        public static string GetCurrentEnvironmentType()
        {
            return ReadJsonFile(GetConfigFilePath()).SelectToken("environment").SelectToken("current").ToString().Trim();
        }
        public static string GetTestType()
        {
            return ReadJsonFile(GetConfigFilePath()).SelectToken("testsuite").SelectToken("testtype").ToString().Trim();
        }

        public static string GetAPIEndpoint()
        {
            return ReadJsonFile(GetConfigFilePath()).SelectToken("url").SelectToken("api").ToString().Trim();
        }

        public static BrowserType GetBrowser(string browser)
        {
            return browser.ToLower().Trim() switch
            {
                "chrome" => BrowserType.Chrome,
                "firefox" => BrowserType.Firefox,
                "safari" => BrowserType.Safari,
                "edge" => BrowserType.Edge,
                _ => BrowserType.Chrome
            };

        }

        public static string GetEnvironment()
        {
            return FileSystem.GetCurrentEnvironmentType();
        
        }
        public static string GetDriverPath()
        {
            return Path.Combine(GetCurrentWorkingDir(), "Drivers");
        }
        public static string GetReportPath()
        {
            return Path.Combine(GetCurrentWorkingDir(), "Reports");
        }
        public static string GetScreenshotPath()
        {
            return Path.Combine(GetCurrentWorkingDir(),"Reports","Screenshots");
        }

        public static string GetApiRequestFilePath()
        {
            return Path.Combine(GetCurrentWorkingDir(), "Requests");
        }
        public static int GetDefaultWaitTime()
        {
            return Int32.Parse(ReadJsonFile(GetConfigFilePath()).SelectToken("defaultwait").SelectToken("wait").ToString().Trim());
        }
    }
}
