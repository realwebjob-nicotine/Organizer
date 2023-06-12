using Caliburn.Micro;
using Organizer.Models;
using Organizer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
        private int id;

        public BaseDocument Document
        {
            get => document;
            set
            {
                document = value;

                Id = document.Id;
                Name = document.Name;
                Description = document.Description;
                Signature = (Guid)document.Signature;

                NotifyOfPropertyChange(() => document);
            }
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid Signature { get; set; }
        public Enums.WindowMode Mode { get; set; }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            GenerateId();
        }

        public void Save()
        {
            var caption = "Редактирование документа";

            var document = new BaseDocument()
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Type = Enums.Type.Document,
                Signature = Signature
            };

            //TODO: на рефакторинг, повторяющийся код можно вынести в сервис
            if (Mode == Enums.WindowMode.Create)
            {
                if (Model.ExistsId(Id))
                {
                    MessageBox.Show("Измените идентификатор, такой идентификатор уже есть!", caption, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (Model.AddDocument(document))
                    {
                        var message = "Документ добавлен, закрыть окно?";
                        if (MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            Cancel();
                        }
                    }
                }
            }
            else
            {
                if (Model.UpdateDocument(document))
                {
                    var message = "Документ обновлен, закрыть окно?";
                    if (MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        Cancel();
                    }
                }
            }
        }

        public void Sign()
        {
            var newSign = Guid.NewGuid();

            while (Model.ExistsSignature(newSign))
            {
                newSign = Guid.NewGuid();
            }

            Signature = newSign;
        }

        private void GenerateId()
        {
            if (Id == default)
            {
                Id = Model.GetMaxId();
                Id++;
            }
        }

        public void Cancel()
        {
            TryCloseAsync();
        }
    }
}
