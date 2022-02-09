using System;
using System.Collections.Generic;

namespace PermissionsN5.Core.Entities
{
    public partial class Permissions : BaseEntity
    {
        public string EmployeeForename { get; set; }
        public string EmployeeSurname { get; set; }
        public int PermissionType { get; set; }
        public DateTime PermissionDate { get; set; }

        public virtual PermissionsTypes PermissionTypeNavigation { get; set; }
    }
}
