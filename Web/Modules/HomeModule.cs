using Memorialis.Core.Sys.Settings;
using Nancy;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace efe.Memorialis.Web.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => { return View["Default"]; };
            Get["/Empty"] = _ => {
                return Response.AsText("", "text/html");
            };
            Get["/Echo"] = _ => {
                Dictionary<string, string> result = new Dictionary<string, string>();
                result.Add("code", Request.Query["code"]);
                return Response.AsJson(result); 
            };
            Get["/Settings.js"] = _ => {
                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                string result = JsonConvert.SerializeObject(Settings.Current.Export(), Formatting.None, settings);
                result = "define(" + result + ");";
                return Response.AsText(result, "application/javascript");
            };
        }

    }
}