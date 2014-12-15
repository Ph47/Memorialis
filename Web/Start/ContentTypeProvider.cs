using Microsoft.Owin.StaticFiles.ContentTypes;

namespace Memorialis.Web
{
    /// <summary>
    /// Strict content type provider
    /// </summary>
    class ContentTypeProvider : FileExtensionContentTypeProvider
    {
        public ContentTypeProvider()
        {
            Mappings.Clear();
            Mappings.Add(".html", "text/html");
            Mappings.Add(".js", "application/javascript");
        }
    }
}