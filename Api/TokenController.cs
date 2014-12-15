using Memorialis.Core.Sys.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace efe.Memorialis.Api
{
    /// <summary>
    /// OAuth token proxy
    /// </summary>
    public class TokenController : ApiController
    {
        private string echoUrl;
        private string tokenUrl;

        /// <summary>
        /// Initialize token and echo urls from settings
        /// </summary>
        private void InitUrls()
        {
            UriBuilder uriBuilder = new UriBuilder(Settings.Current["ProjectUrl"]);
            uriBuilder.Path = Settings.Current["EchoUrl"];
            echoUrl = Uri.EscapeDataString(uriBuilder.ToString());
            uriBuilder.Path = Settings.Current["TokenEndpointPath"];
            tokenUrl = uriBuilder.ToString();
        }

        /// <summary>
        /// Query authorization server for tokens by authorization code
        /// </summary>
        /// <param name="code">Authorization code</param>
        /// <returns>JSON serialized authorization server response</returns>
        [HttpGet]
        public HttpResponseMessage Get(string code)
        {
            InitUrls();
            string parameters = "grant_type=authorization_code&code={code}&redirect_uri={url}&client_id={client}&client_secret={secret}";
            parameters = parameters
                .Replace("{code}", Uri.EscapeDataString(code))
                .Replace("{url}", echoUrl)
                .Replace("{client}", Settings.Current["ClientId"])
                .Replace("{secret}", Settings.Current["Secret"]);

            return GetResponse(tokenUrl, parameters);      
        }

        /// <summary>
        /// Query authorization server for tokens by refresh token
        /// </summary>
        /// <param name="token">Refresh token</param>
        /// <returns>JSON serialized authorization server response</returns>
        [HttpGet]
        public HttpResponseMessage Refresh(string token)
        {
            InitUrls();
            string parameters = "grant_type=refresh_token&refresh_token={code}&redirect_uri={url}&client_id={client}&client_secret={secret}";            
            parameters = parameters
                .Replace("{code}", Uri.EscapeDataString(token))
                .Replace("{url}", echoUrl)
                .Replace("{client}", Settings.Current["ClientId"])
                .Replace("{secret}", Settings.Current["Secret"]);
            return GetResponse(tokenUrl, parameters);            
        }

        /// <summary>
        /// Send request and pack response
        /// </summary>
        private HttpResponseMessage GetResponse(string address, string parameters)
        {

            using (WebClient client = new WebClient())
            {
                //Disable certificate validation
                //TODO: rewrite to specified certificates
                System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string json = client.UploadString(address, parameters);
                HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                return response;
            }

        }
    }
}
