using Memorialis.Core;
using Memorialis.Core.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;



namespace Memorialis.Identity
{
    /// <summary>
    /// Domain specific OAuth server provider
    /// </summary>
    public class ServerProvider : OAuthAuthorizationServerProvider
    {
        public ServerProvider()
            : base()
        {
            this.OnValidateClientRedirectUri = ProviderValidateClientRedirectUri;
            this.OnValidateClientAuthentication = ProviderValidateClientAuthentication;
            this.OnAuthorizeEndpoint = ProviderAuthorizeEndpoint;

            //Unsupported grants
            this.OnGrantClientCredentials = ProviderGrantClientCredentials;
            this.OnGrantCustomExtension = ProviderGrantCustomExtension;
            this.OnGrantResourceOwnerCredentials = ProviderGrantResourceOwnerCredentials;
            

        }

        #region Unsupported grants


        private Task ProviderGrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            context.SetError("specified grant_type not supported");
            return Task.FromResult(0);
        }
        private Task ProviderGrantCustomExtension(OAuthGrantCustomExtensionContext context)
        {
            context.SetError("specified grant_type not supported");
            return Task.FromResult(0);
        }
        private Task ProviderGrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.SetError("specified grant_type not supported");
            return Task.FromResult(0);
        }
        #endregion

        /// <summary>
        /// Primary authorization method
        /// </summary>
        /// 
        private async Task ProviderAuthorizeEndpoint(OAuthAuthorizeEndpointContext context)
        {
            //read login
            IFormCollection form = await context.Request.ReadFormAsync();
            string login = form["login"];
            if (!string.IsNullOrWhiteSpace(login))
            {
                
                UserManager identityManager = new UserManager(new UserStore());

                User user = null;

                //find user by login
                user = await identityManager.FindByNameAsync(login);

                //find user by email
                if (user == null)
                    user = await identityManager.FindByEmailAsync(login);

                //find user by phone
                if (user == null)
                    user = await identityManager.FindByPhoneAsync(login);
                if (user != null)
                {
                    //validate password
                    if (await identityManager.CheckPasswordAsync(user, form["password"]))
                    {

                        //if password is OK generate identity with claims
                        ClaimsIdentity identity = new ClaimsIdentity("Bearer");
                        identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName));
                        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

                        foreach (string role in await identityManager.GetRolesAsync(user.Id))
                        {
                            identity.AddClaim(new Claim(ClaimTypes.Role, role));
                        }

                        //sign in user
                        context.OwinContext.Authentication.SignIn(identity);
                    }
                }
            }
            //and break owin processing
            context.RequestCompleted();
        }

        /// <summary>
        /// Validate client redirect url
        /// </summary>
        private async Task ProviderValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            using (Context db = Context.New())
            {
                //take client by client_id
                Client client = await db.Clients.FirstOrDefaultAsync(c=>c.ClientId == context.ClientId);
                //validate url from request with stored one
                if (client != null)
                    if (client.RedirectUrl == context.RedirectUri)
                        context.Validated(client.RedirectUrl);
            }
        }

        /// <summary>
        /// Validate client secret
        /// </summary>
        private async Task ProviderValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;

            //get credentials from POST only
            if (context.TryGetFormCredentials(out clientId, out clientSecret))
            {
                using (Context db = Context.New())
                {
                    //take client by client_id and validate itssecret
                    Client client = await db.Clients.FirstOrDefaultAsync(c => c.ClientId == context.ClientId);
                    if (client.Secret == clientSecret)
                        context.Validated(client.ClientId);
                }
            }           
        }


    }
}