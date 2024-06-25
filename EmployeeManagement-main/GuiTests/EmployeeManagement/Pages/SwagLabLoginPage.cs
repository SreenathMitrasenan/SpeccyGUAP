using OpenQA.Selenium;
namespace EmployeeManagement.StepDefinitions
{


public class SwagLabLoginPage
{
	private readonly IWebDriver _driver;
	public SwagLabLoginPage(IWebDriver driver)  => _driver = driver;

	public IWebElement userName => _driver.FindElement(By.Id( "user-name" ));
	public IWebElement password => _driver.FindElement(By.Id( "password" ));
	public IWebElement login => _driver.FindElement(By.Id( "login-button" ));
	public IWebElement swaglabs => _driver.FindElement(By.XPath( "//div[text()='Swag Labs']" ));
	public object[] GetControlInfo(string key)
	{
		Dictionary<string, object[]> controls = new Dictionary<string, object[]>();
		controls.Add("userName", new object[]{"Username", "Textbox", "SendKeys", By.Id("user-name")});
		controls.Add("password", new object[]{"Password", "Textbox", "SendKeys", By.Id("password")});
		controls.Add("login", new object[]{"Login", "Button", "Click", By.Id("login-button")});
		controls.Add("swaglabs", new object[]{"Swag Labs", "Label", "Verify", By.XPath("//div[text()='Swag Labs']")});
	if (controls.ContainsKey(key))
	return controls[key];
	else 
	return null; 
	}

	public IWebElement GetWebElement(string key)
	{
		Dictionary<string, IWebElement> elementDictionary = new Dictionary<string, IWebElement>();
		elementDictionary.Add("userName", userName);
		elementDictionary.Add("password", password);
		elementDictionary.Add("login", login);
		elementDictionary.Add("swaglabs", swaglabs);
	  return elementDictionary.TryGetValue(key, out IWebElement webElement) ? webElement : null;
	}

}
}

