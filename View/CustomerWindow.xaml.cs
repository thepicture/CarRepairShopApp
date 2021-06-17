using CarRepairShopApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        private int pageNum = 0;
        /// <summary>
        /// Filtration for the service view.
        /// </summary>
        private void UpdateServiceView(object sender, RoutedEventArgs e)
        {
            List<Service> services = Manager.Context.Service.ToList();
            services = services.Where(s => s.SE_NAME.ToLower().Contains(TBoxServiceName.Text.ToLower())).ToList();
            if (ComboCar.SelectedIndex != 0 && ComboModel.SelectedItem != null)
            {
                services = services.Where(s => s.ServiceOfModel.Select(t => t.TypeOfCar).Contains(ComboModel.SelectedItem as TypeOfCar)).ToList();
            }
            services = services.Skip(pageNum * 20).Take(20).ToList();
            ServicesList.ItemsSource = services;
            DatabaseServiceCount.Text = "Выведено " + services.Count + " услуг из " + Manager.Context.Service.Count();
        }

        /// <summary>
        /// Opens a new window with additional information about the selected service.
        /// </summary>
        private void BtnMoreInfo_Click(object sender, RoutedEventArgs e)
        {
            Service service = (sender as Button).DataContext as Service;
            ServiceInfoWindow infoWindow = new ServiceInfoWindow(service)
            {
                Title = "Информация об услуге " + service.SE_NAME
            };
            infoWindow.ShowDialog();
        }

        /// <summary>
        /// Closes the current application.
        /// </summary>
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

        /// <summary>
        /// Makes a new customer order.
        /// </summary>
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

        /// <summary>
        /// Checks if a customer chose auto and its model.
        /// </summary>
        private void ServicesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboCar.SelectedIndex != 0)
            {
                ComboModel.ItemsSource = Manager.Context.TypeOfCar.ToList().Where(t => t.Auto.Equals(ComboCar.SelectedItem as Auto)).ToList();
                ComboModel.IsEnabled = true;
            }
            else
            {
                ComboModel.IsEnabled = false;
                ComboModel.SelectedItem = null;
            }
            if (ComboCar.SelectedIndex != 0 && ServicesList.SelectedItems.Count > 0 && ComboModel.SelectedItem != null)
            {
                BtnMakeAnOrder.Visibility = Visibility.Visible;
            }
            else
            {
                BtnMakeAnOrder.Visibility = Visibility.Hidden;
            }
            UpdateServiceView(null, null);
        }

        /// <summary>
        /// Navigates to the previous page.
        /// </summary>
        private void BtnPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (pageNum > 0)
            {
                pageNum--;
            }
            BtnFirst.Content = pageNum + 1;
            BtnSecond.Content = pageNum + 2;
            BtnThird.Content = pageNum + 3;
            int countOfPages = Manager.Context.Service.Count() / 20 + 1;
            if (Convert.ToInt32(BtnSecond.Content) < countOfPages)
            {
                BtnSecond.IsEnabled = true;
            }
            if (Convert.ToInt32(BtnThird.Content) < countOfPages)
            {
                BtnThird.IsEnabled = true;
            }
            UpdateServiceView(null, null);
        }

        /// <summary>
        /// Navigates to the first page.
        /// </summary>
        private void BtnFirst_Click(object sender, RoutedEventArgs e)
        {
            BtnPreviousPage_Click(null, null);
        }

        /// <summary>
        /// Navigates to a second page.
        /// </summary>
        private void BtnSecond_Click(object sender, RoutedEventArgs e)
        {
            pageNum = Convert.ToInt32(BtnSecond.Content) - 1;

            BtnFirst.Content = (pageNum + 1).ToString();
            BtnSecond.Content = (pageNum + 2).ToString();
            BtnThird.Content = (pageNum + 3).ToString();

            int countOfPages = (Manager.Context.Service.Count() / 20) + 1;
            if (Convert.ToInt32(BtnSecond.Content) > countOfPages)
            {
                BtnSecond.IsEnabled = false;
            }

            if (Convert.ToInt32(BtnThird.Content) > countOfPages)
            {
                BtnThird.IsEnabled = false;
            }
            UpdateServiceView(null, null);
        }

        /// <summary>
        /// Navigates to a third page.
        /// </summary>
        private void BtnThird_Click(object sender, RoutedEventArgs e)
        {
            pageNum = Convert.ToInt32(BtnThird.Content) - 1;

            BtnFirst.Content = (pageNum + 1).ToString();
            BtnSecond.Content = (pageNum + 2).ToString();
            BtnThird.Content = (pageNum + 3).ToString();

            int countPage = (Manager.Context.Service.Count() / 20) + 1;
            if (Convert.ToInt32(BtnSecond.Content) > countPage)
            {
                BtnSecond.IsEnabled = false;
            }

            if (Convert.ToInt32(BtnThird.Content) > countPage)
            {
                BtnThird.IsEnabled = false;
            }
            UpdateServiceView(null, null);
        }

        /// <summary>
        /// Navigates to a next page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (ServicesList.Items.Count >= 20)
            {
                pageNum++;
            }
            BtnFirst.Content = pageNum + 1;
            BtnSecond.Content = pageNum + 2;
            BtnThird.Content = pageNum + 3;
            int countOfPages = Manager.Context.Service.Count() / 20 + 1;
            if (Convert.ToInt32(BtnSecond.Content) > countOfPages)
            {
                BtnSecond.IsEnabled = false;
            }
            if (Convert.ToInt32(BtnThird.Content) > countOfPages)
            {
                BtnThird.IsEnabled = false;
            }
            UpdateServiceView(null, null);
        }

        /// <summary>
        /// Updates navigation bar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboCar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pageNum = 0;
            BtnFirst_Click(null, null);
            ServicesList_SelectionChanged(null, null);
        }

        /// <summary>
        /// Shows hints in the StatusBar.
        /// </summary>
        private void MainGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Source is Button button)
            {
                if (button.ToolTip != null)
                {
                    ButtonName.Text = button.ToolTip.ToString();
                }
                else
                {
                    ButtonName.Text = button.Content.ToString();
                }
            }
        }

        /// <summary>
        /// Make the StatusBar empty.
        /// </summary>
        private void MainGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonName.Text = null;
        }

        private void TBoxServiceName_TextChanged(object sender, TextChangedEventArgs e)
        {
            BtnFirst_Click(null, null);
            UpdateServiceView(null, null);
        }
    }
}
