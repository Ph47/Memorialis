using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memorialis.Core.Base
{
    public abstract class ListItem<T> : BaseEntity<T> where T : ListItem<T>
    {
        [Index]
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        [Index]
        public int Weight { get; set; }
    }
}
