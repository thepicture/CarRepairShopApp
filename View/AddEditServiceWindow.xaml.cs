using CarRepairShopApp.Model;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CarRepairShopApp.View
{
    /// <summary>
    /// Interaction logic for AddEditServiceWindow.xaml
    /// </summary>
    public partial class AddEditServiceWindow : Window
    {
        private readonly Service _currentService = new Service();
        public AddEditServiceWindow(Service service)
        {
            InitializeComponent();
            if (service != null)
            {
                _currentService = service;
                ServicePhotoView.ItemsSource = _currentService.PhotoOfService.ToList();
            }
            DataContext = _currentService;
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

        private void BtnAddServicePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if ((bool)dialog.ShowDialog())
            {
                try
                {
                    _currentService.PhotoOfService.Add(new PhotoOfService
                    {
                        PHOTO = File.ReadAllBytes(dialog.FileName)
                    });
                    ServicePhotoView.ItemsSource = _currentService.PhotoOfService.ToList();
                    MessageBox.Show("Изображение успешно прикреплено!",
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

        private void BtnDeleteServicePhoto_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Точно удалить выбранное фото?",
                "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                _currentService.PhotoOfService.Remove((sender as Button).DataContext as PhotoOfService);
                MessageBox.Show("Выбранное фото успешно удалено!",
                    "Успешно!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                ServicePhotoView.ItemsSource = _currentService.PhotoOfService.ToList();
            }
        }

        private void BtnRegisterServiceToCar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSaveService_Click(object sender, RoutedEventArgs e)
        {
            if (_currentService.SE_ID.Equals(0))
            {
                Manager.Context.Service.Add(_currentService);
            }
            try
            {
                Manager.Context.SaveChanges();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Title + " неуспешно! " + ex.Message);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Точно отменить "
                + Title.ToLower()
                + "?",
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