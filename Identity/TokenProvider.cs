using Memorialis.Core;
using Memorialis.Core.Identity;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Memorialis.Identity
{
    /// <summary>
    /// Multi purpose token provider
    /// </summary>
    public class TokenProvider : IAuthenticationTokenProvider
    {
        /// <summary>
        /// Specify token type
        /// </summary>
        private Token.TokenType Type { get; set; }

        /// <summary>
        /// Initiate provider
        /// </summary>
        /// <param name="type">Token type</param>
        public TokenProvider(Token.TokenType type)
        {
            Type = type;
        }

        /// <summary>
        /// Create token
        /// </summary>
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            using (Context db = new Context())
            {
                //compose tokent
                Token token = new Token(Type)
                {
                    //lurk for true user id
                    UserId = new Guid(
                        context.Ticket.Identity.Claims
                        .Where(c => c.Type == ClaimTypes.NameIdentifier)
                        .Select(c => c.Value)
                        .FirstOrDefault()
                        ),
                    //store ticket
                    Ticket = context.SerializeTicket(),
                    //and dates from ticket
                    Issued = context.Ticket.Properties.IssuedUtc.Value.UtcDateTime,
                    Expired = context.Ticket.Properties.ExpiresUtc.Value.UtcDateTime,
                    Used = false

                };

                //prolongate refresh tokens for month
                if (Type == Token.TokenType.RefreshToken)
                    token.Expired = token.Issued.AddMonths(1);

                //store ticket
                db.Tokens.Add(token);
                await db.SaveChangesAsync();

                //send ticket
                context.SetToken(token.Code);
            }
        }

        /// <summary>
        /// Acept token
        /// </summary>
        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            //get code
            string codeString = context.Token;

            //and date
            DateTime now = DateTime.UtcNow;

            using (Context db = new Context())
            {
                //look for right token
                Token token = await db.Tokens
                    .Where(t => t.Type == Type)
                    .Where(t => t.Code == codeString)
                    .Where(t => t.Expired > now)
                    .Where(t => !t.Used)
                    .FirstOrDefaultAsync();
                //break if none found
                if (token == null) return;

                //mark and save token
                token.Used = true;
                await db.SaveChangesAsync();

                //load ticket
                context.DeserializeTicket(token.Ticket);                
            }
        }

        /// <summary>
        /// Method not implemented
        /// </summary>        
        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method not implemented
        /// </summary>        
        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }
    }
};