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

        public BindableCollection<StateExtended> States { get; set; }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            GenerateId();
            LoadStates();

            if (Mode == Enums.WindowMode.Create)
            {
                SelectedState = States.FirstOrDefault();
            }
        }

        public void Save()
        {
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

        private void LoadStates()
        {
            var tempStates = Enum.GetValues(typeof(Enums.State))
                .Cast<Enums.State>()
                .Select(s => new StateExtended() { State = s, Name = GetEnumDescription(s) });

            States = new BindableCollection<StateExtended>(tempStates);
        }

        private string GetEnumDescription(Enum value)
        {
            // Get the Description attribute value for the enum value
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
