using Memorialis.Core;
using Memorialis.Core.Identity;
using Memorialis.Core.Sys.Settings;
using Memorialis.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Security.OAuth;
using MVCControlsToolkit.Owin.Globalization;
using Nancy;
using Nancy.Owin;
using Owin;
using SquishIt.Framework;
using SquishIt.Less;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Hosting;
using System.Web.Http;


namespace Memorialis.Web
{
    public class Startup
    {
        /// <summary>
        /// Configuration startpoint
        /// </summary>
        public void Configuration(IAppBuilder app)
        {
            InitializeEnvironment();

            app.UseGlobalization(GlobalizationConfig);
            app.UseOAuthBearerAuthentication(BearerAuthenticationConfig);
            app.UseOAuthAuthorizationServer(AuthorizationServerConfig);
            app.UseWebApi(WebApiConfig);
            app.UseNancy(NancyConfig);
            app.UseStageMarker(PipelineStage.MapHandler);

            using (Context db = new Context())
            {
                db.Database.CreateIfNotExists();
            }
            
            
        }

        /// <summary>
        /// Initialize global environment
        /// </summary>
        private void InitializeEnvironment()
        {
            
            string root = HostingEnvironment.MapPath("~");
            if (string.IsNullOrEmpty(root))
            {
                var uriPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                root = new Uri(uriPath).LocalPath;
            }
            if (Settings.Current["RootPath"] != root)
            {
                Settings.Current["RootPath"] = root;
                Settings.Current.Save();
            }
        }

        /// <summary>
        /// Globalization configuration
        /// </summary>
        private OwinGlobalizationOptions GlobalizationConfig
        {
            get
            {
                OwinGlobalizationOptions config = new OwinGlobalizationOptions("ru-RU",true);
                config.CustomCookieName("CurrentCulture");
                config.DisableCultureInPath();
                return config;
            }
        }


        /// <summary>
        /// Nancy configuration
        /// </summary>
        private NancyOptions NancyConfig
        {
            get
            {
                NancyOptions config = new NancyOptions();
                config.PassThroughWhenStatusCodesAre(HttpStatusCode.NotFound);
                Bundle.RegisterStylePreprocessor(new LessPreprocessor());
                return config;
            }
        }

        /// <summary>
        /// Web Api configuration
        /// </summary>
        private HttpConfiguration WebApiConfig
        {
            get
            {
                HttpConfiguration config = new HttpConfiguration();
                config.Routes.MapHttpRoute(
                    name: "Api",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );
                config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

                return config;
            }
        }
                
        /// <summary>
        /// Authorization Server configuration
        /// </summary>
        private OAuthAuthorizationServerOptions AuthorizationServerConfig
        {
            get
            {
                OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions()
                {
                    AuthorizeEndpointPath = new PathString(Settings.Current["AuthorizeEndpointPath"]),
                    TokenEndpointPath = new PathString(Settings.Current["TokenEndpointPath"]),
                    AllowInsecureHttp = false,
                    AuthorizationCodeExpireTimeSpan = TimeSpan.FromMinutes(5),
                    AccessTokenExpireTimeSpan = TimeSpan.FromHours(1),
                    Provider = new ServerProvider(),
                    AuthorizationCodeProvider = new TokenProvider(Token.TokenType.AuthorizationCode),
                    //uncomment string below to enable server-based access tokens
                    //AccessTokenProvider = new  TokenProvider(Token.TokenType.AccessToken),
                    RefreshTokenProvider = new TokenProvider(Token.TokenType.RefreshToken),
                };
                return options;
            }
        }

        /// <summary>
        /// Bearer configuration
        /// </summary>
        private OAuthBearerAuthenticationOptions BearerAuthenticationConfig
        {
            get
            {
                OAuthBearerAuthenticationOptions options = new OAuthBearerAuthenticationOptions();
                {
                    
                };
                return options;
            }
        }
    }


}