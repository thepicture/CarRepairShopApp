using CarRepairShopApp.Model;
using System;
using System.IO;
using System.Linq;
using System.Windows;
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
            AddEditServiceWindow addEditServiceWindow = new AddEditServiceWindow(null);
            addEditServiceWindow.ShowDialog();
        }
    }
}
