using Memorialis.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memorialis.Core.Identity
{
    public class Token : BaseEntity<Token>
    {
        protected Token()
        {
        }

        public Token(TokenType type)
        {
            Type = type;
            this.FillToken();
        }

        public TokenType Type { get; set; }

        [Required]
        [MaxLength(32)]
        [Index]
        public string Code { get; protected set; }

        public string Ticket { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public DateTime Issued { get; set; }

        public DateTime Expired { get; set; }

        public bool Used { get; set; }

        protected void FillToken()
        {
            if (Code != null)
                throw new InvalidOperationException("Cant set Token, it's not empty");
            if (Id == Guid.Empty) Id = Guid.NewGuid();
            string code = Convert.ToBase64String(Id.ToByteArray());
            code = code.Substring(0, code.Length - 2);
            Code = code;
        }

        public enum TokenType
        {
            AuthorizationCode, AccessToken, RefreshToken
        }
    }


}
