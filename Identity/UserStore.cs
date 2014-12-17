using Memorialis.Core;
using Memorialis.Core.Identity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Memorialis.Identity
{
    /// <summary>
    /// Domain specific IUserStore implementation
    /// </summary>
    public class UserStore : 
        IUserStore<User, Guid>, 
        IUserPasswordStore<User, Guid>, 
        IUserPhoneNumberStore<User, Guid>, 
        IUserEmailStore<User, Guid>, 
        IUserRoleStore<User, Guid>
    {

        public UserStore()
        {
        }

        #region IUserStore
        public async Task CreateAsync(User user)
        {
            user.FillId();
            using (Context db = new Context())
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(User user)
        {
            using (Context db = new Context())
            {
                db.Users.Attach(user);
                db.Users.Remove(user);
                await db.SaveChangesAsync();
            }
        }

        public async Task<User> FindByIdAsync(Guid userId)
        {
            using (Context db = new Context())
            {
                return await db.Users.FindAsync(userId);
            }
        }

        public async Task<User> FindByNameAsync(string userName)
        {
            string loweredUserName = userName.ToLower();
            using (Context db = new Context())
            {
                return await db.Users
                    .FirstOrDefaultAsync(u => u.UserName.ToLower() == loweredUserName);
            }
        }

        public async Task UpdateAsync(User user)
        {
            using (Context db = new Context())
            {
                db.Users.Attach(user);
                await db.SaveChangesAsync();
            }
        }

        public void Dispose()
        {
            return;
        }
        #endregion

        #region IUserPasswordStore
        public async Task<string> GetPasswordHashAsync(User user)
        {            
            using (Context db = new Context())
            {
                return await db.Users
                    .Where(u => u.Id==user.Id)
                    .Select(u=> u.PasswordHash)
                    .FirstAsync();
            }
        }

        public async Task<bool> HasPasswordAsync(User user)
        {
            using (Context db = new Context())
            {
                return await db.Users
                    .Where(u => u.Id == user.Id)
                    .Select(u => u.PasswordHash != null)
                    .FirstAsync();
            }
        }

        public async Task SetPasswordHashAsync(User user, string passwordHash)
        {
            using (Context db = new Context())
            {
                User usr = await db.Users.SingleAsync(u => u.Id == user.Id);//Attach(user);
                usr.PasswordHash = passwordHash;
                await db.SaveChangesAsync();
            }
        }
        #endregion

        #region IUserPhoneNumberStore
        public async Task<string> GetPhoneNumberAsync(User user)
        {
            using (Context db = new Context())
            {
                return await db.Users
                    .Where(u => u.Id == user.Id)
                    .Select(u => u.Phone)
                    .FirstAsync();
            }
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(User user)
        {
            using (Context db = new Context())
            {
                return await db.Users
                    .Where(u => u.Id == user.Id)
                    .Select(u => u.PhoneConfirmed)
                    .FirstAsync();
            }
        }

        public async Task SetPhoneNumberAsync(User user, string phoneNumber)
        {
            using (Context db = new Context())
            {
                db.Users.Attach(user);
                user.Phone = phoneNumber;
                await db.SaveChangesAsync();
            }
        }

        public async Task SetPhoneNumberConfirmedAsync(User user, bool confirmed)
        {
            using (Context db = new Context())
            {
                db.Users.Attach(user);
                user.PhoneConfirmed = confirmed;
                await db.SaveChangesAsync();
            }
        }

        public async Task<User> FindByPhoneAsync(string phone)
        {
            string loweredPhone = phone.ToLower();
            using (Context db = new Context())
            {
                return await db.Users
                    .FirstOrDefaultAsync(u => u.Phone.ToLower() == loweredPhone);
            }
        }
        #endregion

        #region IUserEmailStore
        public async Task<User> FindByEmailAsync(string email)
        {
            string loweredEmail = email.ToLower();
            using (Context db = new Context())
            {
                return await db.Users
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == loweredEmail);
            }
        }

        public async Task<string> GetEmailAsync(User user)
        {
            using (Context db = new Context())
            {
                return await db.Users
                    .Where(u => u.Id == user.Id)
                    .Select(u => u.Email)
                    .FirstAsync();
            }
        }

        public async Task<bool> GetEmailConfirmedAsync(User user)
        {
            using (Context db = new Context())
            {
                return await db.Users
                    .Where(u => u.Id == user.Id)
                    .Select(u => u.EmailConfirmed)
                    .FirstAsync();
            }
        }

        public async Task SetEmailAsync(User user, string email)
        {
            using (Context db = new Context())
            {
                db.Users.Attach(user);
                user.Email = email;
                await db.SaveChangesAsync();
            }
        }

        public async Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            using (Context db = new Context())
            {
                db.Users.Attach(user);
                user.EmailConfirmed = confirmed;
                await db.SaveChangesAsync();
            }
        }
        #endregion


        #region IUserRoleStore
        public async Task AddToRoleAsync(User user, string roleName)
        {
            string loweredRole = roleName.ToLower();
            using (Context db = new Context())
            {
                db.Users.Attach(user);
                Role role = await db.Roles.SingleOrDefaultAsync(r=>r.Name.ToLower() == loweredRole);
                role.Users.Add(user);
                await db.SaveChangesAsync();
            }            
        }

        public async Task<IList<string>> GetRolesAsync(User user)
        {
            Guid userId = user.Id;
            using (Context db = new Context())
            {
                db.Users.Attach(user);
                return await db.Entry<User>(user)
                    .Collection<Role>(r => r.Roles)
                    .Query()
                    .Select(r => r.Name)
                    .ToListAsync();               
            }
        }

        public async Task<bool> IsInRoleAsync(User user, string roleName)
        {
            string loweredRole = roleName.ToLower();
            using (Context db = new Context())
            {
                db.Users.Attach(user);
                return await db.Entry<User>(user)
                    .Collection<Role>(r => r.Roles)
                    .Query()
                    .Where(r => r.Name.ToLower() == loweredRole)
                    .CountAsync() != 0;                    
            }
        }

        public async Task RemoveFromRoleAsync(User user, string roleName)
        {
            string loweredRole = roleName.ToLower();            
            using (Context db = new Context())
            {
                db.Users.Attach(user);
                Role role = await db.Roles.SingleAsync(r => r.Name.ToLower() == roleName);
                user.Roles.Remove(role);
                await db.SaveChangesAsync();
            }
            
        }
        #endregion
    }
}
