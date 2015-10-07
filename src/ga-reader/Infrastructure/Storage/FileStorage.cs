using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ga_reader.Infrastructure.Storage
{
    public class FileStorage<T> where T : class
    {
        private readonly string _fileName;

        public FileStorage(string fileName)
        {
            _fileName = fileName;
        }

        public async Task Save(T obj, bool append = false)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            using (var writer = new StreamWriter(_fileName, append))
            {
                await writer.WriteAsync(json);
            }
        }

        public T Recover()
        {
            var fi = new FileInfo(_fileName);
            if (!fi.Exists)
                return default(T);

            var content = File.ReadAllText(_fileName);
            return JsonConvert.DeserializeObject<T>(content);
        }

        public void Clear()
        {
            var fi = new FileInfo(_fileName);
            fi.Delete();
        }
    }
}