using System;
using System.Collections.Generic;
using System.Text;

namespace PermissionsN5.Core.DTO
{
    public class PermissionsDTO
    {
        public int Id { get; set; }
        public string EmployeeForename { get; set; }
        public string EmployeeSurname { get; set; }
        public int PermissionType { get; set; }
        public DateTime PermissionDate { get; set; }
    }
}
