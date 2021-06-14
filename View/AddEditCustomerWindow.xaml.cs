using CarRepairShopApp.Model;
using CarRepairShopApp.ViewModel;
using System.Windows;

namespace CarRepairShopApp.View
{
    /// <summary>
    /// Interaction logic for AddEditCustomerWindow.xaml
    /// </summary>
    public partial class AddEditCustomerWindow : Window
    {
        private readonly Client _currentCustomer = new Client();
        public AddEditCustomerWindow(Client client)
        {
            InitializeComponent();
            if (client != null)
            {
                _currentCustomer = client;
            }
            DataContext = _currentCustomer;
        }

        private void AttachCustomerPhoto_Click(object sender, RoutedEventArgs e)
        {
            if (PhotoGetter.OpenDialog())
            {
                _currentCustomer.CL_PHOTO = PhotoGetter.ImageInBytes;
                CustomerPhoto.Source = PhotoGetter.Image;
                PhotoGetter.SayAllOk();
            }
        }
    }
}
