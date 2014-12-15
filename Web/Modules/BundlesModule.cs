using Nancy;
using SquishIt.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;

namespace Memorialis.Web.Modules
{
    public class BundlesModule : NancyModule
    {
        public BundlesModule()
        {
           /* Get["/Scripts/{name}"] =
                parameters =>
                    Response
                    .AsText(Bundle.JavaScript().RenderCached((string)parameters.name), Configuration.Instance.JavascriptMimeType)
                    .WithHeader("Cache-Control", "max-age=604800");*/


            /*Get["/Styles/{name}"] =
                parameters =>
                    Response
                    .AsText(Bundle.Css().RenderCached((string)parameters.name), Configuration.Instance.CssMimeType)
                    .WithHeader("Cache-Control", "max-age=604800");
           /* (new SquishIt.Framework.Minifiers.JavaScript.MsMinifier).;*/

            Get["/Styles/{name}"] = param =>
            {
                string name = (string)param.name;
                if (name.EndsWith("less.squishit.debug.css"))
                    return Response.AsFile("Styles/" + name);
                else
                    return Response
                    .AsText(Bundle.Css().RenderCached(name), Configuration.Instance.CssMimeType)
                    .WithHeader("Cache-Control", "max-age=604800");               
            };
        }             
    }
}