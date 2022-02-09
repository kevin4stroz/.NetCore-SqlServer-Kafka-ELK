using System;
using System.Collections.Generic;

namespace PermissionsN5.Core.Entities
{
    public partial class PermissionsTypes : BaseEntity
    {
        public PermissionsTypes()
        {
            Permissions = new HashSet<Permissions>();
        }

        public string Description { get; set; }

        public virtual ICollection<Permissions> Permissions { get; set; }
    }
}
