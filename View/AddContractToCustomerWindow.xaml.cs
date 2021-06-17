using CarRepairShopApp.Model;
using System;
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
            UpdateDataGrids();
        }

        /// <summary>
        /// Updates the datagrids.
        /// </summary>
        private void UpdateDataGrids()
        {
            ContractsOfCustomersGrid.ItemsSource = Manager.CurrentContract.Client.ToList();
            CustomerGrid.ItemsSource = Manager.Context.Client.ToList().Except(Manager.CurrentContract.Client).ToList();
        }

        /// <summary>
        /// Add selected client to current contract.
        /// </summary>
        private void BtnAddSelectedClientsToContract_Click(object sender, RoutedEventArgs e)
        {
            foreach (var client in CustomerGrid.SelectedItems.Cast<Client>().ToList())
            {
                Manager.CurrentContract.Client.Add(client);
            }
            UpdateDataGrids();
        }

        /// <summary>
        /// Updates add button.
        /// </summary>
        private void CustomerGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnAddSelectedClientsToContract.IsEnabled = true;
        }

        /// <summary>
        /// Closes the current window.
        /// </summary>
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

        /// <summary>
        /// Updates the delete button.
        /// </summary>
        private void ContractsOfCustomersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnDeleteCustomerFromContract.IsEnabled = true;
        }

        /// <summary>
        /// Deletes customer from current contract.
        /// </summary>
        private void BtnDeleteCustomerFromContract_Click(object sender, RoutedEventArgs e)
        {
            foreach (var client in ContractsOfCustomersGrid.SelectedItems.Cast<Client>().ToList())
            {
                Manager.CurrentContract.Client.Remove(client);
            }
            BtnDeleteCustomerFromContract.IsEnabled = false;
            UpdateDataGrids();
        }
    }
}
