using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memorialis.Core.Base
{
    public class FilteredDbSet<TEntity> : DbSet<TEntity> where TEntity : TrackableEntity
    {
        protected FilteredDbSet()
            : base()
        {
            
        }
    }
}
