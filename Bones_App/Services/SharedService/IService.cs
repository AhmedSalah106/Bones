namespace Bones_App.Services.SharedService
{
    public interface IService<T> where T : class
    {
        List<T> GetAll();
        T GetById(int Id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(int Id);
    }
}