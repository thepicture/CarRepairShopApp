using CarRepairShopApp.Model;
using Microsoft.Win32;
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
            MastersGrid.ItemsSource = Manager.Context.Master.ToList();
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

        private void ServiceGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ServiceGrid.SelectedItems.Count > 1)
            {
                BtnEditService.IsEnabled = ModifyServiceItem.IsEnabled = false;
            }
            else
            {
                BtnEditService.IsEnabled = ModifyServiceItem.IsEnabled = true;
            }
            BtnDeleteService.IsEnabled = DeleteServiceItem.IsEnabled = true;
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
                        ModifyServiceItem.IsEnabled = DeleteServiceItem.IsEnabled = false;
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

        private void MainAdminWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddServiceItem_Click(object sender, RoutedEventArgs e)
        {
            BtnAddService_Click(null, null);
        }

        private void ModifyServiceItem_Click(object sender, RoutedEventArgs e)
        {
            BtnEditService_Click(null, null);
        }

        private void DeleteServiceItem_Click(object sender, RoutedEventArgs e)
        {
            BtnDeleteService_Click(null, null);
        }

        private void MastersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MastersGrid.SelectedItems.Count > 1)
            {
                BtnEditMaster.IsEnabled = EditMasterItem.IsEnabled = false;
            }
            else
            {
                BtnDeleteMaster.IsEnabled = DeleteMasterItem.IsEnabled = true;
            }
        }

        private void BtnDeleteMaster_Click(object sender, RoutedEventArgs e)
        {
            List<Master> masters = MastersGrid.SelectedItems.Cast<Master>().ToList();
            if (MessageBox.Show("Действительно удалить следующее число мастеров: "
               + masters.Count
               + "?"
               + "\nЭто действие отменить нельзя!",
               "Внимание",
               MessageBoxButton.YesNo,
               MessageBoxImage.Warning)
                == MessageBoxResult.Yes)
            {
                Manager.Context.Master.RemoveRange(masters);
                try
                {
                    Manager.Context.SaveChanges();
                    MastersGrid.ItemsSource = Manager.Context.Master.ToList();
                    MessageBox.Show("Выбранные мастеры успешно удалены!",
                        "Успешно!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Данные не были удалены! Пожалуйста, попробуйте ещё раз." +
                        "\nОшибка:"
                        + ex.Message,
                        "Ошибка удаления",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }
    }
}
