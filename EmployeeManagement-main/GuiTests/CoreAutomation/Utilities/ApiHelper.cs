using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Threading.Tasks;

public static class ApiHelper
{

    public static async Task<RestResponse> PostRequest(string url, JObject body)
    {
        var client = new RestClient(url);
        var request = new RestRequest(url, Method.Post);
        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        foreach (var property in body.Properties())
        { request.AddParameter(property.Name, property.Value.ToString());        
        }
        var response = await client.ExecuteAsync(request);
        return response;

    }

    public static async Task<RestResponse> GetRequest(string url)
    {
        var client = new RestClient(url);
        var request = new RestRequest(url,Method.Get);
        return await client.ExecuteAsync(request);
    }

    public static async Task<RestResponse> GetAllUsers(string url)
    {
        var client = new RestClient(url);
        var request = new RestRequest(url, Method.Get);
        return await client.ExecuteAsync(request);
    }

    public static async Task<RestResponse> PutRequest(string url, object body)
    {
        var client = new RestClient(url);
        var request = new RestRequest(url, Method.Put);
        request.AddJsonBody(body);
        return await client.ExecuteAsync(request);
    }

    public static async Task<RestResponse> DeleteRequest(string url)
    {
        var client = new RestClient(url);
        var request = new RestRequest(url, Method.Delete);
        return await client.ExecuteAsync(request);
    }
}
