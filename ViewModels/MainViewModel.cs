using Caliburn.Micro;
using Organizer.Models;
using Organizer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Organizer.ViewModels
{
    public class MainViewModel : Conductor<object>
    {
        private readonly IWindowManager windowManager;
        private readonly MainModel model;

        public MainViewModel()
        {
            windowManager = IoC.Get<WindowManager>();
            model = IoC.Get<MainModel>();
        }

        public BindableCollection<BaseDocument> Documents { get; set; }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            LoadDocuments();
        }

        private void LoadDocuments()
        {
            try
            {
                var list = model.GetDocuments();
                Documents = new BindableCollection<BaseDocument>(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Organizer", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void AddDocument()
        {
            try
            {
                windowManager.ShowDialogAsync(IoC.Get<DocumentViewModel>());
                LoadDocuments();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Organizer", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void AddTask()
        {
            try
            {
                windowManager.ShowDialogAsync(IoC.Get<TaskViewModel>());
                LoadDocuments();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Organizer", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Open(BaseDocument document)
        {
            try
            {
                if (document.Type == Enums.Type.Document)
                {
                    var vm = IoC.Get<DocumentViewModel>();
                    vm.Mode = Enums.WindowMode.Update;
                    vm.Document = document;
                    windowManager.ShowDialogAsync(vm);
                }
                else
                {
                    var vm = IoC.Get<TaskViewModel>();
                    vm.Mode = Enums.WindowMode.Update;
                    vm.Document = document;
                    windowManager.ShowDialogAsync(vm);
                }

                LoadDocuments();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Organizer", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Delete(BaseDocument document)
        {
            try
            {
                var messageBoxText = string.Format("Вы действительно хотите удалить документ {0} ?", document.Name);
                var caption = "Удаление документа";
                if (MessageBox.Show(messageBoxText, caption, MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                  MessageBoxResult.Yes)
                {
                    model.DeleteDocument(document);
                    LoadDocuments();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Organizer", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
