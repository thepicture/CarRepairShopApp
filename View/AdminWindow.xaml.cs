using CarRepairShopApp.Model;
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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            InitializeUserPhoto();
            UserNameBlock.Text = Manager.CurrentUser.USER_NAME;
            UserRoleBlock.Text = "Роль: " + Manager.CurrentUser.Role.NAME;
            ServiceGrid.ItemsSource = Manager.Context.Service.ToList();
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
            }
            else
            {
                UserImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/user_picture.png"));
            }
        }

        private void BtnAddService_Click(object sender, RoutedEventArgs e)
        {
            AddEditServiceWindow addEditServiceWindow = new AddEditServiceWindow(null)
            {
                Title = "Добавление новой услуги"
            };
            addEditServiceWindow.ShowDialog();
            UpdateEntries();
        }

        /// <summary>
        /// Shows the button content in the StatusBar.
        /// </summary>
        private void MainGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Source is Button button)
            {
                ButtonName.Text = button.Content.ToString();
            }
        }

        /// <summary>
        /// Make the StatusBar empty.
        /// </summary>
        private void MainGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonName.Text = null;
        }

        private void ServiceGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnEditService.IsEnabled = BtnDeleteService.IsEnabled = true;
        }

        private void BtnEditService_Click(object sender, RoutedEventArgs e)
        {
            Service service = ServiceGrid.SelectedItem as Service;
            AddEditServiceWindow addEditServiceWindow = new AddEditServiceWindow(service)
            {
                Title = "Редактирование услуги " + service.SE_NAME
            };
            addEditServiceWindow.ShowDialog();
            UpdateEntries();
        }

        private void UpdateEntries()
        {
            Manager.Context.ChangeTracker.Entries().ToList().ForEach(s => s.Reload());
            ServiceGrid.ItemsSource = Manager.Context.Service.ToList();
        }

        private void BtnDeleteService_Click(object sender, RoutedEventArgs e)
        {
            List<Service> services = ServiceGrid.SelectedItems.Cast<Service>().ToList();
            if (services.Count() > 0)
            {
                if (MessageBox.Show("Внимание! Будет удалено следующее число услуг: "
                    + services.Count() + $".\n" +
                    $"Нажмите \"Да\", если вы действительно хотите безвозвратно удалить выбранные услуги.",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    services.ForEach(s => Manager.Context.PhotoOfService.RemoveRange(s.PhotoOfService.ToList()));
                    Manager.Context.Service.RemoveRange(services);
                    try
                    {
                        Manager.Context.SaveChanges();
                        UpdateEntries();
                        BtnEditService.IsEnabled = BtnDeleteService.IsEnabled = false;
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
    }
}
