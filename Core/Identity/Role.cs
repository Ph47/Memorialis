using Memorialis.Core.Base;
using Microsoft.AspNet.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memorialis.Core.Identity
{
    public class Role : ListItem<Role>, IRole<Guid>
    {
        public virtual ICollection<User> Users { get; set; }
    }
}
