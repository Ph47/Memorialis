using Memorialis.Core.Identity;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;

namespace Memorialis.Identity
{
    /// <summary>
    /// Domain specific implementation of UserManager
    /// </summary>
    public class RoleManager : RoleManager<Role, Guid>
    {
        public RoleManager(IRoleStore<Role, Guid> store)
            : base(store)
        {
           
        }

        public static RoleManager Create(IRoleStore<Role, Guid> store)
        {
            return new RoleManager(store);
        }
    }
}