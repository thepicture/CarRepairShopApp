using CarRepairShopApp.Model;
using System.Windows;

namespace CarRepairShopApp.View
{
    /// <summary>
    /// Interaction logic for AddEditServiceWindow.xaml
    /// </summary>
    public partial class AddEditServiceWindow : Window
    {
        private Service _currentService = new Service();
        public AddEditServiceWindow(Service service)
        {
            InitializeComponent();
            if (service != null)
            {
                _currentService = service;
            }
            DataContext = service;
        }
    }
}
