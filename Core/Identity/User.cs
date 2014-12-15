using Memorialis.Core.Base;
using Memorialis.Core.Structures;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memorialis.Core.Identity
{
    public class User: BaseEntity<User>, IUser<Guid>
    {
        [MaxLength(32)]
        [MinLength(2)]
        [Required]
        [Index]
        public string UserName { get; set; }

        [MaxLength(128)]
        [JsonIgnore]
        public string PasswordHash { get; set; }

        [NotMapped]
        public string Password { get; set; }

        public FullName FullName { get; set; }

        [Index]
        //[RegularExpression("^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9a-z]((\\.(?!\\.))|[-!#\\$%&\'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-z])@))\"(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9][\\-a-z0-9]{0,22}[a-z0-9]))$")]
        [MaxLength(128)]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        [Index]
        [MaxLength(16)]
        public string Phone { get; set; }

        public bool PhoneConfirmed { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
