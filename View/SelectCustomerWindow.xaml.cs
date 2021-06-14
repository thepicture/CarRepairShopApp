using CarRepairShopApp.Model;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CarRepairShopApp.View
{
    /// <summary>
    /// Interaction logic for SelectCustomerWindow.xaml
    /// </summary>
    public partial class SelectCustomerWindow : Window
    {
        public SelectCustomerWindow()
        {
            InitializeComponent();
            CustomerGrid.ItemsSource = Manager.Context.Client.ToList();
        }

        private void CustomerGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnSelectCustomer.IsEnabled = true;
        }

        private void BtnSelectCustomer_Click(object sender, RoutedEventArgs e)
        {
            Client client = CustomerGrid.SelectedItem as Client;
            if (MessageBox.Show("Точно указать клиента "
                + client.CL_NAME
                + "?",
                "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                Manager.CurrentOrder.Client.Clear();
                Manager.CurrentOrder.Client.Add(client);
                Close();
            }
        }
    }
}
