using SmartFridge.Core.Interfaces;
using System.Collections.Generic;
using System.IO;
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
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _items = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            else
            {
                using (FileStream fs = File.Create(filePath)) ; // Создаём пустой файл, если он отсутствует
                _items = new List<T>();
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
