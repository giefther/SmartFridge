namespace SmartFridge.Core.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        void Add(T item);
        void Remove(T item);
        void Save();
    }
}
