using Newtonsoft.Json;
using Organizer.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Models
{
    public class MainModel
    {
        private string path = "data.json";

        public void AddDocument(BaseDocument document)
        {
            var documents = ReadDocuments();
            documents.Add(document);
            WriteDocuments(documents);
        }

        public void DeleteDocument(BaseDocument document)
        {
            var documents = ReadDocuments();
            documents.Remove(document);
            WriteDocuments(documents);
        }

        public List<BaseDocument> ReadDocuments()
        {
            if (File.Exists(path))
            {
                var jsonString = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<List<BaseDocument>>(jsonString);
            }
            else
            {
                var documents = new List<BaseDocument>();
                WriteDocuments(documents);
                return documents;
            }
        }

        public void WriteDocuments(List<BaseDocument> documents)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(documents));
        }
    }
}
