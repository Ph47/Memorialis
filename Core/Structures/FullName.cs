using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memorialis.Core.Structures
{
    /// <summary>
    /// Man's full name structure
    /// </summary>
    [ComplexType]
    public class FullName
    {
        [MaxLength(32)]
        [Required]
        public string Name { get; set; }

        [MaxLength(32)]
        [Required]
        public string Surename { get; set; }

        [MaxLength(32)]
        public string Patronym { get; set; }

        [MaxLength(32)]
        public string Nick { get; set; }
    }
}
//123456789012345678901234567890