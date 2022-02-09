using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PermissionsN5.Core.Entities;
using PermissionsN5.Core.Interfaces;
using PermissionsN5.Infraestructure.Data;

namespace PermissionsN5.Infraestructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly N5SolucionContext _n5Context;
        private readonly IBaseRepository<Permissions> _permissionRepo;
        private readonly IBaseRepository<PermissionsTypes> _permissionTypesRepo;

        public UnitOfWork(N5SolucionContext ctx)
        {
            _n5Context = ctx;
        }


        public IBaseRepository<Permissions> PermissionRepository => _permissionRepo ?? new BaseRepository<Permissions>(_n5Context);

        public IBaseRepository<PermissionsTypes> PermissionTypeRepository => _permissionTypesRepo ?? new BaseRepository<PermissionsTypes>(_n5Context);


        public void Dispose()
        {
            if (_n5Context == null)
                _n5Context.Dispose();
        }

        public void SaveChanges()
        {
            _n5Context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _n5Context.SaveChangesAsync();
        }
    }
}
