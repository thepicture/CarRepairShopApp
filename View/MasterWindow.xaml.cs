﻿using CarRepairShopApp.Model;
using CarRepairShopApp.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CarRepairShopApp.View
{
    /// <summary>
    /// Interaction logic for MasterWindow.xaml
    /// </summary>
    public partial class MasterWindow : Window
    {
        public MasterWindow()
        {
            InitializeComponent();
            InitializeUserPhoto();
            UserNameBlock.Text = Manager.CurrentUser.USER_NAME;
            UserRoleBlock.Text = "Роль: " + Manager.CurrentUser.Role.NAME;
            UpdateEntries();
        }

        private void UpdateEntries()
        {
            ContractGrid.ItemsSource = Manager.Context.Contract.ToList();
            OrdersGrid.ItemsSource = Manager.Context.Order.ToList();
        }

        private void InitializeUserPhoto()
        {
            if (Manager.CurrentUser.USER_PHOTO != null)
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = new MemoryStream(Manager.CurrentUser.USER_PHOTO);
                image.EndInit();
                UserImage.Source = image;
                DeletePictureItem.IsEnabled = true;
            }
            else
            {
                UserImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/user_picture.png"));
                DeletePictureItem.IsEnabled = false;
            }
        }

        /// <summary>
        /// Shows hints in the StatusBar.
        /// </summary>
        private void MainGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Source is Button button)
            {
                ButtonName.Text = button.Content.ToString();
            }
            if (e.Source is MenuItem menuItem)
            {
                ButtonName.Text = menuItem.Header.ToString();
            }
        }

        /// <summary>
        /// Make the StatusBar empty.
        /// </summary>
        private void MainGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonName.Text = null;
        }

        private void ChangePictureItem_Click(object sender, RoutedEventArgs e)
        {
            PictureChanger.ChangePicture();
            InitializeUserPhoto();
        }

        private void DeletePictureItem_Click(object sender, RoutedEventArgs e)
        {
            PictureChanger.DeletePicture();
            InitializeUserPhoto();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddCustomerItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditCustomerItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteCustomerItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CustomersReportForm_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OrdersReportForm_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAddContract_Click(object sender, RoutedEventArgs e)
        {
            AddContractWindow contractWindow = new AddContractWindow(null)
            {
                Title = "Добавление нового контракта"
            };
            contractWindow.ShowDialog();
            UpdateEntries();
        }

        private void BtnEditContract_Click(object sender, RoutedEventArgs e)
        {
            Contract selectedContract = ContractGrid.SelectedItem as Contract;
            AddContractWindow contractWindow = new AddContractWindow(selectedContract)
            {
                Title = "Изменение договора от даты " + selectedContract.CO_DATE
            };
            contractWindow.ShowDialog();
            UpdateEntries();
        }

        private void BtnDeleteContract_Click(object sender, RoutedEventArgs e)
        {
            List<Contract> contracts = ContractGrid.SelectedItems.Cast<Contract>().ToList();
            if (contracts.Count() > 0)
            {
                if (MessageBox.Show("Внимание! Будет отменено следующее число договоров: "
                    + contracts.Count() + $".\n" +
                    $"Нажмите \"Да\", если вы действительно хотите безвозвратно отменить выбранные договоры.",
                    "Подтверждение отмены",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Manager.Context.Contract.RemoveRange(contracts);
                    try
                    {
                        Manager.Context.SaveChanges();
                        UpdateEntries();
                        BtnEditContract.IsEnabled = EditContractItem.IsEnabled = false;
                        BtnDeleteContract.IsEnabled = DeleteContractItem.IsEnabled = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Удаление неуспешно! Пожалуйста, попробуйте ещё раз." +
                            "\nОшибка: "
                            + ex.Message, "Ошибка удаления",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
        }

        private void ContractGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ContractGrid.SelectedItems.Count > 1)
            {
                BtnEditContract.IsEnabled = EditContractItem.IsEnabled = false;
            }
            else
            {
                BtnEditContract.IsEnabled = EditContractItem.IsEnabled = true;
            }
            BtnDeleteContract.IsEnabled = DeleteContractItem.IsEnabled = true;
        }

        private void BtnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            AddEditOrderWindow orderWindow = new AddEditOrderWindow(null)
            {
                Title = "Формирование нового заказа"
            };
            orderWindow.ShowDialog();
            UpdateEntries();
        }

        private void BtnEditOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDeleteOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OrdersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MasterWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Точно завершить текущую сессию пользователя "
            + Manager.CurrentUser.USER_NAME
            + "?",
            "Внимание",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question)
            != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                Manager.MainLoginRegisterWindow.Show();
            }
        }

        /// <summary>
        /// Saves new status to the database.
        /// </summary>
        private void ComboStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrdersGrid.SelectedItems.Count.Equals(0))
            {
                return;
            }
            try
            {
                Order order = OrdersGrid.SelectedItem as Order;
                Status status = (sender as ComboBox).SelectedItem as Status;
                Manager.Context.Order.Where(o => o.O_ID.Equals(order.O_ID)).First().Status = status;
                Manager.Context.SaveChanges();
                MessageBox.Show("Статус заказа успешно изменён!",
                    "Успешно!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("При изменении статуса произошла ошибка." +
                    "Пожалуйста, попробуйте изменить статус ещё раз.",
                    "Внимание",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
