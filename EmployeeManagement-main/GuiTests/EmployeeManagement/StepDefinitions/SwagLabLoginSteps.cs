
using CoreAutomation.Extensions;
using CoreAutomation.TestFixture;
using CoreAutomation.Utilities;
using EmployeeManagement.Pages;
using OpenQA.Selenium;
using static CoreAutomation.Utilities.ReportLog;
namespace EmployeeManagement.StepDefinitions
{


[Binding]
public class SwagLabLoginSteps
{
	private readonly IWebDriver _driver;
	private readonly SwagLabLoginPage _swaglabloginPage;
	private readonly String className ;
	private readonly int defaultWait;
	public SwagLabLoginSteps(ScenarioContext scenarioContext)
	{
		_driver = scenarioContext.Get<IWebDriver>("driver");
		_swaglabloginPage = new SwagLabLoginPage(_driver);
		className = this.GetType().Name.Replace("Steps","");
		defaultWait = TestSettings.DefaultWaitTime;
	}


	[Then(@"I click on (.*) webelement present in SwagLabLogin page")]
	public void ThenIClickOn_WebelementPresentInSwagLabLoginPage(string webElement)
	{
	string txtToType = string.Empty;
		if (_swaglabloginPage.GetControlInfo(webElement) != null) 
		{
			PerformAction(_swaglabloginPage.GetWebElement(webElement), _swaglabloginPage.GetControlInfo(webElement), txtToType, className);
		}
		else
		{
			ReportLog.ReportStep(Status.Fail, String.Format("Web element {0} not found in {1} page  ", webElement, className));
		}
	}

	[Then(@"I set below values in SwagLabLogin page")]
	public void ThenISetBelowValuesInSwagLabLoginPage(Table table)
	{
		foreach (var row in table.Rows)
			{
				var key = row["key"];
				var value = row["value"].Trim();
				if (_swaglabloginPage.GetControlInfo(key) != null)
				{
					PerformAction(_swaglabloginPage.GetWebElement(key),_swaglabloginPage.GetControlInfo(key), value, className);
				}
				else
				{
					ReportLog.ReportStep(Status.Fail, String.Format("Web element {0} not found in {1} page  ", key, className));
				}
			}
	}

	private void PerformAction(IWebElement iElement, object[] objProperties, string value, string className)
		{
			string controlName = objProperties[0].ToString();
			string controlType = objProperties[1].ToString().ToLower().Trim();
			By locator = (By)objProperties[3];
			string txtToSend = value;
			string pageName = className;
			SeleniumExtensions.WaitForElementToLoad(_driver, locator, controlName, pageName,true,20);
			try
			{
				switch (controlType)
				{
					case "button":
						iElement.ClickElement(controlName, controlType, txtToSend, pageName);
						break;
					case "textbox":
						iElement.SendKeysToElement(controlName, txtToSend, pageName);
						break;
					case "radio":
						iElement.ClickElement(controlName, controlType, txtToSend, pageName);
						break;
					case "dropdown":
						iElement.SelectElement(controlName, "value", txtToSend, pageName);
						break;
					case "":
						ReportLog.ReportStep(Status.Fail, String.Format("Control type not found for element {0} ", controlName));
						break;
				case "default":
					break;
				}
			}
			catch (Exception ex)
			{
				ReportLog.ReportStep(Status.Fail, String.Format("Execption occured while performing action, message {0} ", ex.Message));
			}
		}

	}
}

