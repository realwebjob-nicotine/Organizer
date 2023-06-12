using Newtonsoft.Json;
using Organizer.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Organizer.Models
{
    public class MainModel
    {
        private string path = "data.json";

        /// <summary>
        /// Получение базового документа по идентификатору документа
        /// </summary>
        /// <param name="id">Идентификатор документа</param>
        /// <returns>Базовый документ</returns>
        public BaseDocument GetDocumentById(int id)
        {
            var documents = GetDocuments();
            return documents.SingleOrDefault(document => document.Id == id);
        }

        /// <summary>
        /// Получение базового документа по подписи документа (только для типа: документ)
        /// </summary>
        /// <param name="sign">Подпись (guid)</param>
        /// <returns>Базовый документ</returns>
        public BaseDocument GetDocumentBySignature(Guid sign)
        {
            var documents = GetDocuments();
            var searchedDocument = documents
              .SingleOrDefault(document => document.Type == Enums.Type.Document && document.Signature == sign);
            return searchedDocument;
        }

        /// <summary>
        /// Добавление документа
        /// </summary>
        /// <param name="document">Базовый документ</param>
        /// <returns>В случае успеха метод вернет - true</returns>
        public bool AddDocument(BaseDocument document)
        {
            var documents = GetDocuments();
            documents.Add(document);
            SaveDocuments(documents);
            return true;
        }

        /// <summary>
        /// Обновление документа
        /// </summary>
        /// <param name="updatedDocument">Базовый документ</param>
        /// <returns>В случае успеха метод вернет - true</returns>
        public bool UpdateDocument(BaseDocument updatedDocument)
        {
            var document = GetDocumentById(updatedDocument.Id);
            DeleteDocument(document);
            AddDocument(updatedDocument);
            return true;
        }

        /// <summary>
        /// Удаление документа
        /// </summary>
        /// <param name="document">Базовй документ</param>
        public void DeleteDocument(BaseDocument document)
        {
            var documents = GetDocuments();
            var documentRemove = documents.Single(doc => doc.Id == document.Id);
            documents.Remove(documentRemove);
            SaveDocuments(documents);
        }

        /// <summary>
        /// Получение всех документов
        /// </summary>
        /// <returns>Список документов</returns>
        public List<BaseDocument> GetDocuments()
        {
            if (File.Exists(path))
            {
                var jsonString = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<List<BaseDocument>>(jsonString);
            }
            else
            {
                var documents = new List<BaseDocument>();
                SaveDocuments(documents);
                return documents;
            }
        }

        /// <summary>
        /// Сохранение всех документов
        /// </summary>
        /// <param name="documents">Список документов</param>
        public void SaveDocuments(List<BaseDocument> documents)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(documents));
        }

        /// <summary>
        /// Проверка существования указанного идентификатора среди существующих
        /// </summary>
        /// <param name="id">Идентификатор базового документа</param>
        /// <returns>Если идентификатор существует то - true</returns>
        public bool ExistsId(int id)
        {
            var document = GetDocumentById(id);
            return document != null;
        }

        /// <summary>
        /// Проверка существования указанной подписи среди существующих
        /// </summary>
        /// <param name="guid">Подпись документа</param>
        /// <returns>Если подпись существует то - true</returns>
        public bool ExistsSignature(Guid guid)
        {
            var document = GetDocumentBySignature(guid);
            return document != null;
        }

        /// <summary>
        /// Получить максимальных идентификатор среди существующих
        /// </summary>
        /// <returns>Идентификатор</returns>
        public int GetMaxId()
        {
            var documents = GetDocuments();

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
