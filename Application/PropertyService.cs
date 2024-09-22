using Domain;
using Microsoft.AspNetCore.SignalR;


// Import the namespace where NotificationHub is located

namespace Application
{
    public class PropertyService : IPropertyService<Property>
    {
        private readonly InterfaceGeneric<Property> _genericRepository;
        private readonly InterfaceProperty _propertyRepository;
        private readonly INotificationService _notificationService;
       
        public PropertyService(
            InterfaceGeneric<Property> genericRepository,
            InterfaceProperty propertyRepository,
            INotificationService notificationService)
        {
            _genericRepository = genericRepository;
            _propertyRepository = propertyRepository;
            _notificationService = notificationService;
        }

        public void Add(Property entity)
        {
            _genericRepository.Add(entity);

            // Notify clients via the notification service
            _notificationService.SendNotificationAsync($"New property added in area: {entity.Area}");
        }

        public bool Delete(Property entity)
        {
            return _genericRepository.Delete(entity);
        }

        public bool Update(Property entity)
        {
            return _genericRepository.Update(entity);
        }

        public IEnumerable<Property> GetAll()
        {
            return _genericRepository.GetAll();
        }

        public List<Property> searchProperty(string propertyLocation)
        {
            return _propertyRepository.searchProperty(propertyLocation);
        }
        public List<Property> GetRecentProperties(DateTime lastCheckedTime)
        {
            return _propertyRepository.GetPropertiesAddedAfter(lastCheckedTime);
        }
    }
}
