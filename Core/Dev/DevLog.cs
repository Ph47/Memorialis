using Memorialis.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memorialis.Core.Dev
{
    public class DevLog: BaseEntity<DevLog>
    {
        public DateTime Date { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }
    }
}
