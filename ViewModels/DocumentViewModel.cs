using Caliburn.Micro;
using Organizer.Models;
using Organizer.Types;
using System;
using System.Windows;

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
        public bool IdIsReadOnly { get; set; }
        public bool SaveButtonEnabled => Id != default && !string.IsNullOrEmpty(Name) && Signature != Guid.Empty;

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            GenerateId();

            if (Mode == Enums.WindowMode.Create)
            {
                IdIsReadOnly = false;
            }
            else
            {
                IdIsReadOnly = true;
            }
        }

        public void Save()
        {
            try
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
                        var message = "Измените идентификатор, такой идентификатор уже есть!";
                        MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        if (Model.AddDocument(document))
                        {
                            var message = "Документ добавлен, закрыть окно?";
                            if (MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                              MessageBoxResult.Yes)
                            {
                                Cancel();
                            }
                            else
                            {
                                Id++;
                                Sign();
                            }
                        }
                    }
                }
                else
                {
                    if (Model.UpdateDocument(document))
                    {
                        var message = "Документ обновлен!";
                        MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
                        Cancel();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Organizer", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Sign()
        {
            try
            {
                var newSign = Guid.NewGuid();

                while (Model.ExistsSignature(newSign))
                {
                    newSign = Guid.NewGuid();
                }

                Signature = newSign;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Organizer", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GenerateId()
        {
            try
            {
                if (Id == default)
                {
                    Id = Model.GetMaxId();
                    Id++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Organizer", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Cancel()
        {
            TryCloseAsync();
        }
    }
}
