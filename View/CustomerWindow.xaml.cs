using CarRepairShopApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CarRepairShopApp.View
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        public CustomerWindow()
        {
            InitializeComponent();
            ServicesList.ItemsSource = Manager.Context.Service.ToList();
            List<Auto> autos = Manager.Context.Auto.ToList();
            autos.Insert(0, new Auto { A_NAME = "Не указано" });
            ComboCar.ItemsSource = autos;
        }

        private void UpdateServiceView(object sender, RoutedEventArgs e)
        {
            List<Service> services = Manager.Context.Service.ToList();
            if (ComboCar.SelectedIndex != 0)
            {

            }
            services = services.Where(s => s.SE_NAME.ToLower().Contains(TBoxServiceName.Text.ToLower())).ToList();
            ServicesList.ItemsSource = services;
        }

        private void BtnMoreInfo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Точно покинуть окно просмотра услуг?",
                "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                Close();
                Manager.MainLoginRegisterWindow.Show();
            }
        }

        private void BtnMakeAnOrder_Click(object sender, RoutedEventArgs e)
        {
            ClientDataWindow clientWindow = new ClientDataWindow(ServicesList.SelectedItems.Cast<Service>().ToList(), ComboModel.SelectedItem as TypeOfCar)
            {
                Title = "Оформление заявки на оказание услуг"
            };
            clientWindow.ShowDialog();
            ServicesList.SelectedItems.Clear();
            BtnMakeAnOrder.Visibility = Visibility.Hidden;
        }

        private void ServicesList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ComboCar.SelectedIndex != 0)
            {
                ComboModel.ItemsSource = Manager.Context.TypeOfCar.ToList().Where(t => t.Auto.Equals(ComboCar.SelectedItem as Auto)).ToList();
                ComboModel.IsEnabled = true;
            }
            else
            {
                ComboModel.IsEnabled = false;
            }
            if (ComboCar.SelectedIndex != 0 && ServicesList.SelectedItems.Count > 0 && ComboModel.SelectedItem != null)
            {
                BtnMakeAnOrder.Visibility = Visibility.Visible;
            }
            else
            {
                BtnMakeAnOrder.Visibility = Visibility.Hidden;
            }
        }
    }
}
