using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using Newtonsoft.Json;

namespace BookCollection.Data
{
    public class Database
    {
        private readonly string _dataPath;

        public Database()
        {
            _dataPath = Path.Combine(Application.StartupPath, "Data");
            Directory.CreateDirectory(_dataPath);
        }

        private string GetFilePath(string fileName)
        {
            return Path.Combine(_dataPath, fileName);
        }

        public List<Models.Book> LoadBooks()
        {
            string filePath = GetFilePath("books.json");
            if (!File.Exists(filePath)) return new List<Models.Book>();

            try
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<Models.Book>>(json) ?? new List<Models.Book>();
            }
            catch
            {
                return new List<Models.Book>();
            }
        }

        public void SaveBooks(List<Models.Book> books)
        {
            string filePath = GetFilePath("books.json");
            string json = JsonConvert.SerializeObject(books, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public List<Models.User> LoadUsers()
        {
            string filePath = GetFilePath("users.json");
            if (!File.Exists(filePath)) return new List<Models.User>();

            try
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<Models.User>>(json) ?? new List<Models.User>();
            }
            catch
            {
                return new List<Models.User>();
            }
        }

        public void SaveUsers(List<Models.User> users)
        {
            string filePath = GetFilePath("users.json");
            string json = JsonConvert.SerializeObject(users, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}