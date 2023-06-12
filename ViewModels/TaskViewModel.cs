using Caliburn.Micro;
using Organizer.Models;
using Organizer.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
                var tempState = (Enums.State)document.State;
                SelectedState = new StateExtended()
                {
                    State = tempState,
                    Name = GetEnumDescription(tempState)
                };

                NotifyOfPropertyChange(() => document);
            }
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Type Type { get; set; }
        public StateExtended SelectedState { get; set; }
        public Enums.WindowMode Mode { get; set; }
        public bool IdIsReadOnly { get; set; }
        public bool SaveButtonEnabled => Id != default && !string.IsNullOrEmpty(Name);
        public BindableCollection<StateExtended> States { get; set; }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            GenerateId();
            LoadStates();

            if (Mode == Enums.WindowMode.Create)
            {
                SelectedState = States.FirstOrDefault();
                IdIsReadOnly = false;
            }
            else
            {
                SelectedState = States.Single(s => s.State == SelectedState.State);
                IdIsReadOnly = true;
            }
        }

        public void Save()
        {
            try
            {
                var caption = "Редактирование задачи";

                var document = new BaseDocument()
                {
                    Id = Id,
                    Name = Name,
                    Description = Description,
                    Type = Enums.Type.Task,
                    State = SelectedState.State
                };

                //TODO: на рефакторинг, повторяющийся код можно вынести в сервис
                if (Mode == Enums.WindowMode.Create)
                {
                    if (Model.ExistsId(Id))
                    {
                        MessageBox.Show("Измените идентификатор, такой идентификатор уже есть!", caption,
                          MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        if (Model.AddDocument(document))
                        {
                            var message = "Задача добавлена, закрыть окно?";
                            if (MessageBox.Show(message, caption, MessageBoxButton.YesNo,
                              MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                Cancel();
                            }
                            else
                            {
                                Id++;
                            }
                        }
                    }
                }
                else
                {
                    if (Model.UpdateDocument(document))
                    {
                        var message = "Задача обновлена!";
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

        private void LoadStates()
        {
            try
            {
                var tempStates = Enum.GetValues(typeof(Enums.State))
                    .Cast<Enums.State>()
                    .Select(s => new StateExtended() { State = s, Name = GetEnumDescription(s) });

                States = new BindableCollection<StateExtended>(tempStates);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Organizer", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetEnumDescription(Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute),
              false);

            if (attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public void Cancel()
        {
            TryCloseAsync();
        }
    }
}
