using Memorialis.Core;
using Memorialis.Core.Dev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace efe.Memorialis.Api
{
    [Authorize]
    public class DevLogController : ApiController
    {
        
        [HttpGet]
        public IEnumerable<DevLog> Get()
        {
            using (Context db = new Context())
            {
                return db.DevLogs.OrderByDescending(l => l.Date).ToList();
            }
        }        
    }
}
