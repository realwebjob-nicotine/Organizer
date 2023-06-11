using Caliburn.Micro;
using Organizer.Models;
using Organizer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.ViewModels
{
    public class TaskViewModel : Screen
    {
        public TaskViewModel()
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
                State = document.State;

                NotifyOfPropertyChange(() => document);
            }
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Type Type { get; set; }
        public Enum State { get; set; }
        public Enums.WindowMode Mode { get; set; }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            GenerateId();
        }

        public void Save()
        {
            var document = new BaseDocument()
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Type = Enums.Type.Task,
                State = State
            };

            //TODO: на рефакторинг, повторяющийся код можно вынести в сервис
            if (Mode == Enums.WindowMode.Create)
            {
                if (Model.ExistsId(Id))
                {
                    // message
                }
                else
                {
                    Model.AddDocument(document);
                }
            }
            else
            {
                Model.UpdateDocument(document);
            }
        }

        private void GenerateId()
        {
            if (Id == default)
            {
                Id = Model.GetMaxId();
                Id++;
            }
        }
    }
}
