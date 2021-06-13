using CarRepairShopApp.Model;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CarRepairShopApp.View
{
    /// <summary>
    /// Interaction logic for AddContractToCustomerWindow.xaml
    /// </summary>
    public partial class AddContractToCustomerWindow : Window
    {
        public AddContractToCustomerWindow()
        {
            InitializeComponent();
            CustomerGrid.ItemsSource = Manager.Context.Client.ToList();
            ContractsOfCustomersGrid.ItemsSource = Manager.CurrentContract.Client.ToList();
        }

        private void BtnAddSelectedClientsToContract_Click(object sender, RoutedEventArgs e)
        {
            foreach (var client in CustomerGrid.SelectedItems.Cast<Client>().ToList())
            {
                Manager.CurrentContract.Client.Add(client);
            }
            ContractsOfCustomersGrid.ItemsSource = Manager.CurrentContract.Client.ToList();
        }

        private void CustomerGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnAddSelectedClientsToContract.IsEnabled = true;
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Точно закрыть окно указания клиентов для текущего контракта?",
                "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        private void ContractsOfCustomersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnDeleteCustomerFromContract.IsEnabled = true;
        }

        private void BtnDeleteCustomerFromContract_Click(object sender, RoutedEventArgs e)
        {
            foreach (var client in ContractsOfCustomersGrid.SelectedItems.Cast<Client>().ToList())
            {
                Manager.CurrentContract.Client.Remove(client);
            }
            ContractsOfCustomersGrid.ItemsSource = Manager.CurrentContract.Client.ToList();
            BtnDeleteCustomerFromContract.IsEnabled = false;
        }
    }
}
