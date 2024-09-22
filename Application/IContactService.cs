using System.Collections.Generic;

namespace Application
{
    public interface IContactService<TEntity>
    {
        void Add(TEntity entity);
        bool Delete(TEntity entity);
        bool Update(TEntity entity);
        IEnumerable<TEntity> GetAll();
    }
}
