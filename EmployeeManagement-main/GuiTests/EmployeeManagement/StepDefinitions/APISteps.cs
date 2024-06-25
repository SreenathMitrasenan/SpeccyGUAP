using CoreAutomation.Utilities;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static CoreAutomation.Utilities.ReportLog;

namespace EmployeeManagement.StepDefinitions
{
    [Binding]
    public class APISteps
    {
        private readonly ScenarioContext _scenarioContext;
        public APISteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;

        }


        [Given(@"I load (.*) api request file and input below parameters")]
        public void GivenILoadApiRequestFileAndInputBelowParameters(string requestFile, Table table)
        {
            JObject iRequest = FileSystem.ReadApiRequestFile(requestFile);
            try
            {
                foreach (var row in table.Rows)
                {
                    string key = row["key"];
                    string value = row["value"];
                    JsonHelper.UpdateJsonValue(iRequest, key, value);
                    ReportLog.ReportStep(Status.Info, $"Updated JSON key '{key}' with value '{value}'");
                }
            }
            catch (Exception ex)
            {
                ReportLog.ReportStep(Status.Fail, $"Error updating JSON values. Error: {ex.Message}");
                throw;
            }
            //ReportLog.ReportStep(Status.Info, string.Format("Json request '{0}' ", iRequest.ToString()));
            _scenarioContext.Set(iRequest, "requestbody");


        }
        [Given(@"I load (.*) request file")]
        public void GivenILoadRequestFile(string requestFile)
        {
            try
            {
                JObject iRequest = FileSystem.ReadApiRequestFile(requestFile);
                _scenarioContext.Set(iRequest, "requestbody");
                ReportLog.ReportStep(Status.Info, $"Successfully read API request file : {requestFile}");

            }
            catch (Exception ex)
            {
                ReportLog.ReportStep(Status.Fail, $"Error reading API request file : {requestFile}. Error : {ex.Message}");
                throw;
            }
        }



        [StepDefinition(@"I execute (.*) api request for (.*) functionality and validate response")]
        public void ThenIExecute_ApiRequestFor_FunctionalityAndValidateResponse(string methodType, string functionality)
        {
            RestResponse iresponse = null;
            //var id = string.Empty;
            //var newurl = string.Empty;
            var url = _scenarioContext.Get<string>("apiEndPoint");
            var body = _scenarioContext.Get<JObject>("requestbody");

            switch (methodType.ToUpper()?.Trim())
            {
                case "POST":

                    iresponse = ApiHelper.PostRequest(url, body).GetAwaiter().GetResult();
                    var responseBody = JObject.Parse(iresponse.Content);
                    _scenarioContext.Set(responseBody["_id"]?.ToString(), "Id");
                    break;

                case "DELETE":

                    var id = _scenarioContext.Get<string>("Id");
                    var newurl= $"{url}/{id}";
                    iresponse = ApiHelper.DeleteRequest(newurl).GetAwaiter().GetResult();
                    break;

                case "GETALLUSERS":

                    iresponse = ApiHelper.GetAllUsers(url).GetAwaiter().GetResult();
                    //ReportLog.ReportStep(Status.Info, $"Response content: {iresponse.Content}");
                    LogAndVerifyUsersNames(iresponse.Content);
                    break;

                case "GET":

                    id = _scenarioContext.Get<string>("Id");
                    newurl = $"{url}?id={id}";
                    iresponse = ApiHelper.GetRequest(url).GetAwaiter().GetResult();
                    LogAndVerifyUsersNames(iresponse.Content);
                    break;

                case "UPDATE":

                    id = _scenarioContext.Get<string>("Id");
                    newurl = $"{url}/{id}";
                    iresponse = ApiHelper.PutRequest(newurl, body).GetAwaiter().GetResult();
                    break;

                default:
                    ReportLog.ReportStep(Status.Fail, $"Unsupported HTTP method: {methodType}");
                    return;
            }

            if (iresponse.StatusCode == HttpStatusCode.OK || iresponse.StatusCode == HttpStatusCode.Created)
            {
                ReportLog.ReportStep(Status.Pass, string.Format("Http status code is '{0}' & description is '{1}' ", iresponse.StatusCode, iresponse.StatusDescription));
            }
            else
            {
                ReportLog.ReportStep(Status.Fail, string.Format("Http status code is '{0}' ", iresponse.StatusCode));
            }
            
        }


        private void LogAndVerifyUsersNames(string responseContent)
        {
            var users = JArray.Parse(responseContent);
            var userNames = users.Select(user => user["name"]?.ToString()).ToList();

            foreach (var userName in userNames)
            {
                ReportLog.ReportStep(Status.Info, $"User present : {userName}");
            }
        }


    }


}
