using SmartFridge.Core.Interfaces;
using System.Text.Json;

namespace SmartFridge.Data.Repositories
{
    public class JsonRepository<T> : IRepository<T> where T : class
    {
        private readonly string _filePath;
        private List<T> _items;

        public JsonRepository(string filePath)
        {
            _filePath = filePath;

            // Проверяем существует ли файл и не пустой ли он
            if (File.Exists(_filePath) && new FileInfo(_filePath).Length > 0)
            {
                try
                {
                    var json = File.ReadAllText(_filePath);
                    _items = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
                }
                catch (JsonException)
                {
                    // Если файл поврежден или содержит невалидный JSON, 
                    // создаем новую коллекцию
                    _items = new List<T>();
                }
            }
            else
            {
                // Создаем пустую коллекцию и пустой файл
                _items = new List<T>();

                // Создаем директорию если не существует
                var directory = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                SaveChanges(); // Создает файл с пустым массивом JSON
            }
        }

        public IEnumerable<T> GetAll() => _items;

        public T? GetById(Guid id) => _items.FirstOrDefault(x =>
            (Guid)x.GetType().GetProperty("Id")!.GetValue(x)! == id);

        public void Add(T entity) => _items.Add(entity);

        public void Update(T entity)
        {
            var idProp = entity.GetType().GetProperty("Id");
            if (idProp == null) return;

            var id = (Guid)idProp.GetValue(entity)!;
            var existing = GetById(id);
            if (existing != null)
            {
                _items.Remove(existing);
                _items.Add(entity);
            }
        }

        public void Delete(Guid id)
        {
            var existing = GetById(id);
            if (existing != null) _items.Remove(existing);
        }

        public void SaveChanges()
        {
            var json = JsonSerializer.Serialize(_items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}