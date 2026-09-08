using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flickstrap.UI.ViewModels.Settings;

namespace Flickstrap.UI.Elements.Settings.Pages
{
    /// <summary>
    /// Interaction logic for FlickstrapPage.xaml
    /// </summary>
    public partial class FlickstrapPage
    {
        public FlickstrapPage()
        {
            DataContext = new FlickstrapViewModel();
            InitializeComponent();
        }
    }
}
