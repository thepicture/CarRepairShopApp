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
            }
        }

        private void BtnMakeAnOrder_Click(object sender, RoutedEventArgs e)
        {
            Order order = new Order();
            if (Manager.Context.Client.Where(c => c.CL_NAME.Equals(Manager.CurrentUser.USER_NAME)).Any().Equals(false))
            {
                Client client = new Client
                {
                    CL_NAME = Manager.CurrentUser.USER_NAME,
                };
                Manager.Context.Client.Add(client);
                Manager.Context.SaveChanges();
            }
            order.O_CREATEDATE = DateTime.Now;
            Random random = new Random();
            order.Master.Add(Manager.Context.Master.ToList().OrderBy(m => random.Next()).First());
            order.Client.Add(Manager.Context.Client.Where(c => c.CL_NAME.Equals(Manager.CurrentUser.USER_NAME)).First());
            order.Status = Manager.Context.Status.Where(s => s.ST_STATE.Contains("в обработке")).FirstOrDefault();
            order.TypeOfCar = (ComboCar.SelectedItem as Auto).TypeOfCar.First();
            foreach (Service service in ServicesList.SelectedItems)
            {
                order.Service.Add(service);
            }
            try
            {
                Manager.Context.Order.Add(order);
                Manager.Context.SaveChanges();
                MessageBox.Show("Заявка успешно подана!",
                    "Успешно!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                ServicesList.SelectedItems.Clear();
                BtnMakeAnOrder.Visibility = Visibility.Hidden;
            }
            catch (Exception)
            {
                MessageBox.Show("Пожалуйста, попробуйте подать заявку ещё раз!",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ServicesList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ComboCar.SelectedIndex != 0)
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
