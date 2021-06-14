﻿using CarRepairShopApp.Model;
using System;
using System.Linq;
using System.Text;
using System.Windows;

namespace CarRepairShopApp.View
{
    /// <summary>
    /// Interaction logic for AddEditOrderWindow.xaml
    /// </summary>
    public partial class AddEditOrderWindow : Window
    {
        private readonly Order _currentOrder = new Order();
        public AddEditOrderWindow(Order order)
        {
            InitializeComponent();
            if (order != null)
            {
                _currentOrder = order;
                GetNameAndCarOfTheCustomerIfExists();
            }
            DataContext = _currentOrder;
            TBoxMasterName.Text = Manager.CurrentUser.USER_NAME;
        }

        private void GetNameAndCarOfTheCustomerIfExists()
        {
            if (_currentOrder.Client.Count != 0)
            {
                CustomerName.Text = _currentOrder.Client.FirstOrDefault().CL_NAME;
                ComboCar.ItemsSource = _currentOrder.Client.FirstOrDefault().Auto.ToList();
            }
        }

        private void BtnSelectCustomer_Click(object sender, RoutedEventArgs e)
        {
            Manager.CurrentOrder = _currentOrder;
            SelectCustomerWindow customerWindow = new SelectCustomerWindow
            {
                Title = "Выбор клиента"
            };
            customerWindow.ShowDialog();
            GetNameAndCarOfTheCustomerIfExists();
        }

        private void BtnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            if (CustomerName.Text.Length == 0)
            {
                builder.AppendLine("Укажите клиента");
            }
            if (ComboCar.SelectedItem is null)
            {
                builder.AppendLine("Укажите автомобиль клиента");
            }
            if (builder.Length > 0)
            {
                MessageBox.Show(builder.ToString(),
                    Title + " неуспешно",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if (_currentOrder.O_ID.Equals(0))
            {
                _currentOrder.Status = Manager.Context.Status.First(s => s.ST_STATE == "В обработке");
                Manager.Context.Order.Add(_currentOrder);
            }
            try
            {
                Manager.Context.SaveChanges();
                MessageBox.Show("Заказ клиента "
                    + _currentOrder.Client.FirstOrDefault().CL_NAME
                    + " успешно сохранен!",
                    "Успешно!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Title + " неуспешно! " + ex.Message);
            }
        }
    }
}
