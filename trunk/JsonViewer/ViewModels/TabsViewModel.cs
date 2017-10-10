using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marss.JsonViewer.ViewModels
{
    public class TabsViewModel
    {
        public ObservableCollection<TabViewModelBase> Tabs { get; private set; }

        public TabsViewModel()
        {
            Tabs = new ObservableCollection<TabViewModelBase>()
            {
                new DefaultViewerTabViewModel(),
                new ComparerTabViewModel(),
                new JsonPathEvaluatorTabViewModel()
            };
        }

        
    }
}
