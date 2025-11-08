using System.Text.Json;
using SmartFridge.Core.Interfaces;

namespace SmartFridge.Data
{
    public class JsonRepository<T> : IRepository<T>
    {
        private readonly string _path;
        private List<T> _items = new();

        public JsonRepository(string path)
        {
            _path = path;
            if (File.Exists(_path))
                _items = JsonSerializer.Deserialize<List<T>>(File.ReadAllText(_path)) ?? new List<T>();
        }

        public IEnumerable<T> GetAll() => _items;

        public void Add(T item)
        {
            _items.Add(item);
            Save();
        }

        public void Remove(T item)
        {
            _items.Remove(item);
            Save();
        }

        public void Save()
        {
            File.WriteAllText(_path, JsonSerializer.Serialize(_items, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
