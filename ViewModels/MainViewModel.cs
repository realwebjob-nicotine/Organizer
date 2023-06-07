using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.ViewModels
{
    public class MainViewModel
    {
        public BindableCollection<BaseDocumentViewModel> Documents { get; set; }
    }
}
