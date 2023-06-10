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
    public class MainViewModel : Conductor<object>, IShell
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

        public void Open(BaseDocument document)
        {
            var vm = IoC.Get<DocumentViewModel>();

            vm.Document = document;

            windowManager.ShowDialogAsync(vm);
            LoadDocuments();
        }
    }
}
