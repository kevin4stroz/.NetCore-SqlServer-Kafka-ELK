using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PermissionsN5.Core.Entities;

namespace PermissionsN5.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Permissions> PermissionRepository { get; }
        IBaseRepository<PermissionsTypes> PermissionTypeRepository { get; }

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
