using Bones_App.Repositories.SharedRepo;

namespace Bones_App.Services.SharedService
{
    public class Service<T> : IService<T> where T : class
    {

        private readonly IRepository<T> repository;
        public Service(IRepository<T>repository)
        {
            this.repository = repository;
        }
        public void Delete(int Id)
        {
            repository.Delete(Id);
        }

        public List<T> GetAll()
        {
            return repository.GetAll();
        }

        public T GetById(int Id)
        {
            return repository.GetById(Id);
        }

        public void Insert(T entity)
        {
            repository.Insert(entity);
        }

        public void Update(T entity)
        {
            repository.Update(entity);
        }
    }
}
