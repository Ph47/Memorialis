using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;
using SquishIt.Framework;

namespace Memorialis.Web
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);


            //Collect all stylesheets
            Bundle.Css()
                .Add("/Styles/Default.less")
                .WithMinifier<SquishIt.Framework.Minifiers.CSS.YuiMinifier>()
                .AsCached("Default", "~/Styles/Default");

            //Prepare RequireJS
            /*Bundle.JavaScript()
                .Add("~/Scripts/RequireJS/RequireJS-2.1.14.js")
                .WithAttribute("data-main","/Bundles/Scripts/Init")
                .AsCached("Require", "~/Bundles/Scripts/Require");*/

            //Prepare Init 
            /*Bundle.JavaScript()
                .Add("~/Scripts/Init.js")
                .AsCached("Init", "~/Bundles/Scripts/Init");*/



            //Bind links to viewbag of every page
            pipelines.BeforeRequest += (ctx) =>
            {
                ctx.ViewBag.Styles = Bundle.Css().RenderNamed("Default");
                //ctx.ViewBag.RequireJS = Bundle.JavaScript().RenderNamed("Require");
                return null;
            };
            pipelines.AfterRequest += (ctx) =>
                {
                    if (ctx.Response.ContentType == "text/html")
                        ctx.Response.ContentType = "text/html; charset=utf8";
                };

        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            //Set culture conventions to use only cookie
            Conventions.CultureConventions.Clear();
            Conventions.CultureConventions.Add(BuiltInCultureConventions.CookieCulture);
            Conventions.StaticContentsConventions.Clear();
            Conventions.StaticContentsConventions.AddDirectory("Scripts");
            Conventions.StaticContentsConventions.AddDirectory("Styles", null, "less");
            Conventions.StaticContentsConventions.AddDirectory("Static");

            //Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("scripts", @"Scripts")
       
            base.ConfigureConventions(nancyConventions);
        }
    }
}