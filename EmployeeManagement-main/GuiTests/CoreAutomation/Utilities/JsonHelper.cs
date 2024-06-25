using Newtonsoft.Json.Linq;
using System.IO;

public static class JsonHelper
{
    public static JObject LoadJson(string filePath)
    {
        var jsonContent = File.ReadAllText(filePath);
        return JObject.Parse(jsonContent);
    }

    public static void ModifyJson(JObject json, Table table)
    {
        foreach (var row in table.Rows)
        {
            var key = row["Key"];
            var value = row["Value"];
            json[key] = value;
        }
    }


    public static void UpdateJsonValue(JObject jsonObject, string keyPath, string newValue)
    {
        try
        {
            JToken token = jsonObject.SelectToken(keyPath);
            if (token != null)
            {
                token.Replace(JToken.FromObject(newValue));
            }
            else
            {
                throw new Exception($"Key '{keyPath}' not found in the JSON structure.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating JSON value: {ex.Message}");
        }
    }





}
