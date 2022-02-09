using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PermissionsN5.Core.Entities;

namespace PermissionsN5.Core.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int Id);
        Task<T> Update(T obj);
        Task<T> Add(T obj);
    }
}
