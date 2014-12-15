using Memorialis.Core.Identity;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;

namespace Memorialis.Identity
{
    /// <summary>
    /// Domain specific implementation of UserManager
    /// </summary>
    public class UserManager : UserManager<User, Guid>
    {
        public UserManager(IUserStore<User, Guid> store)
            : base(store)
        {
           
        }      

        /// <summary>
        /// Find user by phone number
        /// </summary>
        /// <param name="phone">Phone to look by</param>
        public async Task<User> FindByPhoneAsync(string phone)
        {
            UserStore store = Store as UserStore;
            return await store.FindByPhoneAsync(phone);
        }

        /// <summary>
        /// Static initialization method
        /// </summary>
        /// <param name="store">IUserStore</param>
        /// <returns>UserManager</returns>
        public static UserManager Create(IUserStore<User, Guid> store)
        {
            return new UserManager(store);
        }
    }
}