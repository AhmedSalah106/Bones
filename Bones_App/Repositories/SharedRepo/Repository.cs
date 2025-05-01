using Bones_App.Models;

namespace Bones_App.Repositories.SharedRepo
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly BonesContext context;
        public Repository(BonesContext context)
        {
            this.context = context; 
        }

        public void Delete(int Id)
        {
            T entity = GetById(Id);
            context.Remove(entity);
        }

        

        public List<T> GetAll()
        {
            return context.Set<T>().ToList();
        }

        public T GetById(int Id)
        {
            return context.Set<T>().Find(Id);
        }

        public void Insert(T entity)
        {
            context.Add(entity);
        }

        

        public void Update(T entity)
        {
            context.Update(entity);
        }
    }
}
