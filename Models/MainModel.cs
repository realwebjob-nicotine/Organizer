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

        public BaseDocument GetDocumentById(int id)
        {
            var documents = ReadDocuments();
            return documents.SingleOrDefault(document => document.Id == id);
        }

        public BaseDocument GetDocumentBySignature(Guid sign)
        {
            var documents = ReadDocuments();
            var document = documents.SingleOrDefault(document => document.Type == Enums.Type.Document && document.Signature == sign);
            return document;
        }

        public bool AddDocument(BaseDocument document)
        {
            var documents = ReadDocuments();
            documents.Add(document);
            WriteDocuments(documents);
            return true;
        }

        public bool UpdateDocument(BaseDocument updatedDocument)
        {
            var document = GetDocumentById(updatedDocument.Id);
            DeleteDocument(document);
            AddDocument(updatedDocument);
            return true;
        }

        public void DeleteDocument(BaseDocument document)
        {
            var documents = ReadDocuments();
            var documentRemove = documents.Single(doc => doc.Id == document.Id);
            documents.Remove(documentRemove);
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

        public bool ExistsId(int id)
        {
            var document = GetDocumentById(id);
            return document != null;
        }

        public bool ExistsSignature(Guid guid)
        {
            var document = GetDocumentBySignature(guid);
            return document != null;
        }

        public int GetMaxId()
        {
            var documents = ReadDocuments();

            if (documents.Count != 0)
            {
                return documents.Max(document => document.Id);
            }
            else
            {
                return 0;
            }

        }
    }
}
