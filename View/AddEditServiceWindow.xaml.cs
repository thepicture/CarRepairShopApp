using CarRepairShopApp.Model;
using CarRepairShopApp.ViewModel;
using System;
using System.Linq;
using System.Text;
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
        /// Shows hints in the StatusBar.
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
            if (PhotoGetter.OpenDialog())
            {
                _currentService.PhotoOfService.Add(new PhotoOfService
                {
                    PHOTO = PhotoGetter.ImageInBytes
                });
                ServicePhotoView.ItemsSource = _currentService.PhotoOfService.ToList();
                PhotoGetter.SayAllOk();
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
                PhotoOfService service = (sender as Button).DataContext as PhotoOfService;
                _currentService.PhotoOfService.Remove(service);
                Manager.Context.PhotoOfService.Remove(service);
                MessageBox.Show("Выбранное фото успешно удалено!",
                    "Успешно!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                ServicePhotoView.ItemsSource = _currentService.PhotoOfService.ToList();
            }
        }

        private void BtnRegisterServiceToCar_Click(object sender, RoutedEventArgs e)
        {
            Manager.CurrentService = _currentService;
            AddEditCarToServiceWindow carWindow = new AddEditCarToServiceWindow()
            {
                Title = "Добавить автомобиль к услуге " + _currentService.SE_NAME
            };
            carWindow.ShowDialog();
        }

        private void BtnSaveService_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            if (ServiceNameBox.Text.Length == 0)
            {
                builder.AppendLine("Укажите наименование услуги");
            }
            if (builder.Length > 0)
            {
                MessageBox.Show(builder.ToString(),
                    Title + " неуспешно",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if (_currentService.SE_ID.Equals(0))
            {
                Manager.Context.Service.Add(_currentService);
            }
            try
            {
                Manager.Context.SaveChanges();
                MessageBox.Show("Услуга "
                    + ServiceNameBox.Text
                    + " успешно сохранена!",
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
                Manager.Context.Entry(_currentService).State = System.Data.Entity.EntityState.Unchanged;
                Close();
            }
        }
    }
}