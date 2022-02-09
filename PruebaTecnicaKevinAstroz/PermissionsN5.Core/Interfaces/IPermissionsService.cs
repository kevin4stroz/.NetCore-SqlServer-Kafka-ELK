using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PermissionsN5.Core.DTO;

namespace PermissionsN5.Core.Interfaces
{
    public interface IPermissionsService
    {
        Task<PermissionsDTO> Add(ModPermissionDTO obj);
        Task<List<PermissionsDTO>> GetAll();
        Task<PermissionsDTO> Update(int id,ModPermissionDTO modPermission);
    }
}
