using Memorialis.Core;
using Memorialis.Core.Identity;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Memorialis.Identity
{
    /// <summary>
    /// Domain specific IUserStore implementation
    /// </summary>
    public class RoleStore : IRoleStore<Role, Guid>
    {

        public RoleStore()
        {
        }
               

        #region IRoleStore
        public async Task CreateAsync(Role role)
        {
            role.FillId();
            using (Context db = new Context())
            {
                db.Roles.Add(role);
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Role role)
        {
            using (Context db = new Context())
            {
                db.Roles.Attach(role);
                db.Roles.Remove(role);
                await db.SaveChangesAsync();
            }
        }

        public async Task<Role> FindByIdAsync(Guid roleId)
        {
            using (Context db = new Context())
            {
                return await db.Roles.FindAsync(roleId);
            }
        }

        public async Task<Role> FindByNameAsync(string roleName)
        {
            string loweredRoleName = roleName.ToLower();
            using (Context db = new Context())
            {
                return await db.Roles
                    .FirstOrDefaultAsync(r => r.Name.ToLower() == loweredRoleName);
            }
        }

        public async Task UpdateAsync(Role role)
        {
            using (Context db = new Context())
            {
                db.Roles.Attach(role);
                await db.SaveChangesAsync();
            }
        }
        #endregion

        public void Dispose()
        {
            return;
        }
    }
}
