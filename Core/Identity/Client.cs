using Memorialis.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memorialis.Core.Identity
{
    public class Client: BaseEntity<Client>
    {
        public string ClientId { get; set; }

        public string Secret { get; set; }

        public string RedirectUrl { get; set; }
    }
}
