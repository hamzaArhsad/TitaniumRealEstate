using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application
{
    public interface IPropertyService<T> where T : Property
    {
        void Add(T entity);
        bool Delete(T entity);
        bool Update(T entity);
        IEnumerable<T> GetAll();
        List<T> searchProperty(string propertyLocation);
        public List<Property> GetRecentProperties(DateTime lastCheckedTime);
    }
}
