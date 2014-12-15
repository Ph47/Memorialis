using Memorialis.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memorialis.Core.Sys.Settings
{
    public class Setting : BaseEntity<Setting>
    {
        [Index(IsUnique=true)]
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public SettingGroup Group { get; set; }

        public bool IsPublic { get; set; }
    }
}
