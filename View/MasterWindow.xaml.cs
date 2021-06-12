using CarRepairShopApp.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            OpenFileDialog dialog = new OpenFileDialog();
            if ((bool)dialog.ShowDialog())
            {
                try
                {
                    Manager.CurrentUser.USER_PHOTO = File.ReadAllBytes(dialog.FileName);
                    Manager.Context.SaveChanges();
                    InitializeUserPhoto();
                    MessageBox.Show("Аватар успешно изменен!",
                        "Успешно!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Пожалуйста, прикрепите изображение.",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void DeletePictureItem_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Точно удалить текущий аватар?",
              "Внимание",
              MessageBoxButton.YesNo,
              MessageBoxImage.Question)
              == MessageBoxResult.Yes)
            {
                Manager.CurrentUser.USER_PHOTO = null;
                Manager.Context.SaveChanges();
                InitializeUserPhoto();
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddOrderItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ModifyOrderItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteOrderItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddContractItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditContractItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteContractItem_Click(object sender, RoutedEventArgs e)
        {

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

        }

        private void BtnEditContract_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDeleteContract_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ContractGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BtnAddOrder_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
