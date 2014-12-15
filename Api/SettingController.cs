using efe.Memorialis.Api.Filters;
using Memorialis.Core.Identity;
using Memorialis.Core.Structures;
using Memorialis.Core.Sys.Settings;
using Memorialis.Identity;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;

namespace efe.Memorialis.Api
{
  
    public class SettingController : ApiController
    {
        [HttpGet]
        [AllowAnonymous]
        public Dictionary<string,string> Get()
        {
            return Settings.Current.Export();
        }
      
    }
}
