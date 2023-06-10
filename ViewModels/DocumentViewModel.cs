using Caliburn.Micro;
using Organizer.Models;
using Organizer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.ViewModels
{
    public class DocumentViewModel : Screen
    {
        public DocumentViewModel()
        {
            Model = IoC.Get<MainModel>();
        }

        private readonly MainModel Model;

        private BaseDocument document;

        public BaseDocument Document
        {
            get => document;
            set
            {
                document = value;

                Id = document.Id;
                Name = document.Name;
                Description = document.Description;
                Signature = document.Signature;

                NotifyOfPropertyChange(() => document);
            }
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid Signature { get; set; }

        public void Save()
        {
            Model.AddDocument(new BaseDocument()
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Type = Enums.Type.Document,
                Signature = Signature
            });
        }
    }
}
