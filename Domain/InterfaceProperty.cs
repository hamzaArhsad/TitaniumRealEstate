namespace Domain
{
    public interface InterfaceProperty
    {
        public List<Property> searchProperty(string loc);
        public List<Property> GetPropertiesAddedAfter(DateTime lastCheckedTime);
    }
}
