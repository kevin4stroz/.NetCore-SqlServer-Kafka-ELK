using System;
using System.Collections.Generic;
using System.Text;
using PermissionsN5.Core.Interfaces;
using PermissionsN5.Core.Entities;
using System.Threading.Tasks;
using PermissionsN5.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace PermissionsN5.Infraestructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly N5SolucionContext _n5SolutionCtx;
        private DbSet<T> _entities;

        public BaseRepository(N5SolucionContext ctx)
        {
            _n5SolutionCtx = ctx;
            _entities = ctx.Set<T>();
        }

        public async Task<T> Add(T obj)
        {
            try
            {
                await _entities.AddAsync(obj);
                await _n5SolutionCtx.SaveChangesAsync();
                return obj;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<T>> GetAll()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T> GetById(int Id)
        {
            return await _entities.FindAsync(Id);
        }

        public async Task<T> Update(T obj)
        {
            try
            {
                _entities.Update(obj);
                await _n5SolutionCtx.SaveChangesAsync();
                return obj;
            }
            catch
            {
                return null;
            }
        }
    }
}
