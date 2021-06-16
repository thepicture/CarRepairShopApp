using CarRepairShopApp.Model;
using System.Linq;
using System.Windows;

namespace CarRepairShopApp.View
{
    /// <summary>
    /// Interaction logic for ServiceInfoWindow.xaml
    /// </summary>
    public partial class ServiceInfoWindow : Window
    {
        public ServiceInfoWindow(Service service)
        {
            InitializeComponent();
            DataContext = service;
            ServicePhotoView.ItemsSource = service.PhotoOfService.ToList();
        }

        private void BtnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Точно закрыть окно дополнительной информации об услуге?",
                "Подтверждение закрытия",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                Close();
            }
        }
    }
}
