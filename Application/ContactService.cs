using Domain;
using System.Collections.Generic;

namespace Application
{
    public class ContactService : IContactService<Contact>
    {
        private readonly InterfaceGeneric<Contact> _genericRepository;

        public ContactService(InterfaceGeneric<Contact> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        // Implementing methods from IContactService
        public void Add(Contact entity)
        {
            _genericRepository.Add(entity);
        }

        public bool Delete(Contact entity)
        {
            return _genericRepository.Delete(entity);
        }

        public bool Update(Contact entity)
        {
            return _genericRepository.Update(entity);
        }
        public IEnumerable<Contact> GetAll()
        {
            return _genericRepository.GetAll();
        }
    }
}
