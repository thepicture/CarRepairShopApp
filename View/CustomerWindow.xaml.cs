using CarRepairShopApp.Model;
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
            autos.Insert(0, new Auto { A_NAME = "Все автомобили" });
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
            }
        }
    }
}
