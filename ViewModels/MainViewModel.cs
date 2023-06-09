using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.ViewModels
{
    public class MainViewModel : Conductor<object>
    {
        private readonly IWindowManager WindowManager;

        public MainViewModel(IWindowManager windowManager)
        {
            WindowManager = windowManager;
        }

        public BindableCollection<BaseDocumentViewModel> Documents { get; set; }

        protected async override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            Documents = new BindableCollection<BaseDocumentViewModel>();
        }

        public Task AddDocument()
        {
            return WindowManager.ShowDialogAsync(IoC.Get<DocumentViewModel>());
        }
    }
}
