namespace Bones_App.Repositories.SharedRepo
{
    public interface IRepository<T>where T : class
    {
        public List<T> GetAll();
        public T GetById(int Id);
        public void Insert(T entity);
        public void Update(T entity);
        public void Delete(int Id);

    }
}