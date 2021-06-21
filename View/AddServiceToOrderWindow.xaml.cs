using CarRepairShopApp.Model;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CarRepairShopApp.View
{
    /// <summary>
    /// Interaction logic for AddServiceToOrderWindow.xaml
    /// </summary>
    public partial class AddServiceToOrderWindow : Window
    {
        public AddServiceToOrderWindow()
        {
            InitializeComponent();
            UpdateDataGrids();
        }

        private void UpdateDataGrids()
        {
            ServicesOfOrderGrid.ItemsSource = Manager.CurrentOrder.Service.ToList();
            ServiceGrid.ItemsSource = Manager.Context.Service.ToList().Except(Manager.CurrentOrder.Service).ToList();
        }

        private void ServiceGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnAddSelectedServicesToOrder.IsEnabled = true;
        }

        private void BtnAddSelectedServicesToOrder_Click(object sender, RoutedEventArgs e)
        {
            foreach (var service in ServiceGrid.SelectedItems.Cast<Service>().ToList())
            {
                Manager.CurrentOrder.Service.Add(service);
            }
            UpdateDataGrids();
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Точно закрыть окно указания услуг для текущего заказа?",
               "Внимание",
               MessageBoxButton.YesNo,
               MessageBoxImage.Question)
               == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        private void ServicesOfOrderGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnDeleteServicesFromOrder.IsEnabled = true;
        }

        private void BtnDeleteServicesFromOrder_Click(object sender, RoutedEventArgs e)
        {
            foreach (var service in ServicesOfOrderGrid.SelectedItems.Cast<Service>().ToList())
            {
                Manager.CurrentOrder.Service.Remove(service);
            }
            UpdateDataGrids();
            BtnDeleteServicesFromOrder.IsEnabled = false;
        }

        /// <summary>
        /// Finds services by its name.
        /// </summary>
        private void ServiceFinder_TextChanged(object sender, TextChangedEventArgs e)
        {
            ServiceGrid.ItemsSource = Manager.Context.Service.ToList().Where(s => s.SE_NAME.ToLower()
            .Contains(ServiceFinder.Text.ToLower()));
        }

        /// <summary>
        /// Finds services of order by its name.
        /// </summary>
        private void ServiceOfOrderFinder_TextChanged(object sender, TextChangedEventArgs e)
        {
            ServicesOfOrderGrid.ItemsSource = Manager.CurrentOrder.Service.ToList().Where(s => s.SE_NAME.ToLower()
            .Contains(ServiceOfOrderFinder.Text.ToLower()));
        }
    }
}
