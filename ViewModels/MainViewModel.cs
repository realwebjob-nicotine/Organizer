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
            var list = model.ReadDocuments();
            Documents = new BindableCollection<BaseDocument>(list);
        }

        public void AddDocument()
        {
            windowManager.ShowDialogAsync(IoC.Get<DocumentViewModel>());
            LoadDocuments();
        }

        public void AddTask()
        {
            windowManager.ShowDialogAsync(IoC.Get<TaskViewModel>());
            LoadDocuments();
        }

        public void Open(BaseDocument document)
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

        public void Delete(BaseDocument document)
        {
            model.DeleteDocument(document);
            LoadDocuments();
        }
    }
}
