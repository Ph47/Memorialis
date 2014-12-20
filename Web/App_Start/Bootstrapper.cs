using dotless.Core;
using Memorialis.Core.Sys.Settings;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;
using SquishIt.Framework;
using SquishIt.Framework.CSS;
using SquishIt.Framework.JavaScript;
using SquishIt.Framework.Minifiers.CSS;
using System;
using System.IO;
using System.Reflection;
using System.Web.Hosting;
using System.Xml.Linq;

namespace Memorialis.Web
{
    /// <summary>
    /// Nancy configuration class
    /// </summary>
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        //Nancy entry point
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            //Load assets
            ProcessAssets();
            
            pipelines.BeforeRequest += (ctx) =>
            {
                //Load required static resource tags
                ctx.ViewBag.Styles = Bundle.Css().RenderCachedAssetTag("Default");
                ctx.ViewBag.RequireJS = Bundle.JavaScript().RenderCachedAssetTag("Require");

                return null;
            };

            pipelines.AfterRequest += (ctx) =>
            {
                //Fix encoding problem
                if (ctx.Response.ContentType == "text/html")
                    ctx.Response.ContentType = "text/html; charset=utf8";
            };

            base.ApplicationStartup(container, pipelines);
        }

        /// <summary>
        /// Process file "app_data\assets.xml" and load suitable resources
        /// </summary>
        /// <remarks>
        /// This method solve problem of Nancy and SquishIt collaboration. Manualy setting minimized or 
        /// original resource depends on configuration selected. ForceRelease() always called becouse 
        /// SquishIt in debug mode produce tags but not suitable resources.
        /// 
        /// Because of separate classes used by SquishIt for css and js serving an IF with cpypaste present.
        /// We need to fix it.
        /// </remarks>
        private void ProcessAssets()
        {
            //detect configuration
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            //load assets settings
            string root = Settings.Current["RootPath"];
            XDocument doc = XDocument.Load(root + "/App_Data/Assets.xml");

            //iterate assets
            var assets = doc.Root.Elements("asset");
            foreach (var asset in assets)
            {
                //load basic params
                string type = asset.Attribute("type").Value.ToLower();
                string name = asset.Attribute("name").Value;
                string path = asset.Attribute("path").Value;

                //load collections
                var files = asset.Elements("file");
                var attributes = asset.Elements("attribute");

                //type switch
                if (type == "css")
                {
                    CSSBundle bundle = Bundle.Css();

                    //process every file
                    foreach (var file in files)
                    {
                        if (debug)
                        {
                            //block minification by adding source as minimized
                            bundle.AddMinified(file.Attribute("source").Value);
                        }
                        else
                        {
                            //if minimized file present - use it, overwise - minimize
                            if (file.Attribute("minified") != null)
                                bundle.AddMinified(file.Attribute("minified").Value);
                            else
                                bundle.Add(file.Attribute("source").Value);
                        }
                    }
                    //process attributes speification
                    foreach (var attribute in attributes)
                        bundle.WithAttribute(attribute.Attribute("name").Value, attribute.Attribute("value").Value);

                    //force using release config outflanks bug described at header
                    bundle.ForceRelease().AsCached(name, path);
                }

                //like previous type excpet type itself
                if (type == "js")
                {
                    JavaScriptBundle bundle = Bundle.JavaScript();
                    foreach (var file in files)
                    {
                        if (debug)
                        {
                            bundle.AddMinified(file.Attribute("source").Value);
                        }
                        else
                        {
                            if (file.Attribute("minified") != null)
                                bundle.AddMinified(file.Attribute("minified").Value);
                            else
                                bundle.Add(file.Attribute("source").Value);
                        }
                    }
                    foreach (var attribute in attributes)
                        bundle.WithAttribute(attribute.Attribute("name").Value, attribute.Attribute("value").Value);
                    bundle.ForceRelease().AsCached(name, path);
                }
            }

        }

        /// <summary>
        /// Configure Nancy conventions
        /// </summary>
        /// <param name="nancyConventions">Conventions to configure</param>
        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            //Set culture conventions to use only cookie
            Conventions.CultureConventions.Clear();
            Conventions.CultureConventions.Add(BuiltInCultureConventions.CookieCulture);

            //Allow images access
            Conventions.StaticContentsConventions.Clear();
            Conventions.StaticContentsConventions.AddDirectory("Images");
            Conventions.StaticContentsConventions.AddDirectory("Html");
            base.ConfigureConventions(nancyConventions);
        }
    }

}