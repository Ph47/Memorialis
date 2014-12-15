using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memorialis.Core.Base
{
    /// <summary>
    /// Base class for all entities, provide identity and timestamp
    /// </summary>
    public abstract class BaseEntity<T> where T : BaseEntity<T>
    {
        /// <summary>
        /// Primary identity for all entities
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        public T FillId()
        {
            if (Id != Guid.Empty)
                throw new InvalidOperationException("Cant set Id, it's not empty");
            Id = Guid.NewGuid();
            return (T)this;
        }

        [Timestamp]
        [JsonIgnore]
        public Byte[] Timestamp { get; set; }
    }
}
