namespace Domain
{
    public interface InterfaceGeneric<TEntity>
    {
        // TEntity FindById(int id);
        //public void DeleteById(int id);
        public IEnumerable<TEntity> GetAll();
        public void Add(TEntity entity);
        public bool Update(TEntity entity);
        public bool Delete(TEntity entity);

    }
}
