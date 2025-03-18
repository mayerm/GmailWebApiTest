namespace WebApplicationTest.Repositories
{
    public interface IGenericRepository<T>
    {
        Task<T> Get(int id);
        Task<IEnumerable<T>> GetAll();
        Task<int> SaveRange(IEnumerable<T> list);
        Task Update(T t);
        Task Insert(T t);
        Task DeleteRow(int id);
    }
}
