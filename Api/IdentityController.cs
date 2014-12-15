using efe.Memorialis.Api.Filters;
using Memorialis.Core.Identity;
using Memorialis.Core.Structures;
using Memorialis.Identity;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;

namespace efe.Memorialis.Api
{
    [Authorize]
    public class IdentityController : ApiController
    {
        [HttpGet]
        [AllowAnonymous]
        public User Get()
        {
            IPrincipal usr = this.User;
            User user = new User();
            user.FullName = new FullName();
            user.UserName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetFileNameWithoutExtension(Path.GetRandomFileName()); //16 random symbols
            return user;
        }

        [HttpPut]
        [ValidateModel]
        [AllowAnonymous]
        public async Task Put([FromBody] User user)
        {
            UserManager identityManager = new UserManager(new UserStore());
            user.EmailConfirmed = false;
            user.PhoneConfirmed = false;
            user.Timestamp = null;
            user.PasswordHash = null;
            IdentityResult result = null;
            result = await identityManager.CreateAsync(user);
            if (!result.Succeeded) this.InternalServerError();
            result = await identityManager.AddPasswordAsync(user.Id, user.Password);
            if (!result.Succeeded)
            {
                this.InternalServerError();
                await identityManager.DeleteAsync(user);
            }
        }

        /*[HttpGet]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [HttpPut]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete]
        public void Delete(int id)
        {
        } */
    }
}
