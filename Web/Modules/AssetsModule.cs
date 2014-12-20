using Memorialis.Core.Sys.Settings;
using Nancy;
using SquishIt.Framework;
using SquishIt.Framework.CSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;

namespace Memorialis.Web.Modules
{
    public class AssetsModule : NancyModule
    {
        public AssetsModule()            
        {
            int cacheTime = Convert.ToInt32(Settings.Current["StaticFilesCacheTime"]);
            Get["/js/{name}.js"] =
                param =>
                {
                    try
                    {
                        return Response.AsText(Bundle.JavaScript().RenderCached((string)param.name))
                        .WithContentType(Configuration.Instance.JavascriptMimeType)
                        .WithHeader("Cache-Control", "max-age=" + cacheTime.ToString());
                    }
                    catch
                    {
                        return HttpStatusCode.NotFound;
                    }
                };
            Get["/css/{name}.css"] =
                param =>
                {
                    try
                    {
                        return Response.AsText(Bundle.Css().RenderCached((string)param.name))
                        .WithContentType(Configuration.Instance.CssMimeType)
                        .WithHeader("Cache-Control", "max-age=" + cacheTime.ToString());
                    }
                    catch
                    {
                        return HttpStatusCode.NotFound;
                    }
                };
            
        }             
    }
}