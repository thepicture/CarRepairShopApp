using CarRepairShopApp.Model;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CarRepairShopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ComboLogin.ItemsSource = Manager.Context.User.ToList();
            ComboRole.ItemsSource = Manager.Context.Role.ToList();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (ComboLogin.SelectedItem is User currentUser)
            {
                if (PBoxPassword.Password == currentUser.USER_PASSWORD)
                {
                    MessageBox.Show($"Вы успешно авторизованы, "
                        + currentUser.USER_NAME + "!",
                        "Успешно!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Пароль введён неверно!\n" +
                        "Пожалуйста, проверьте введённые данные в поле пароля и попробуйте ещё раз.",
                        "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        readonly Regex loginRegex = new Regex(pattern: @".{1,100}");
        private void TBoxLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (loginRegex.IsMatch(TBoxLogin.Text))
            {
                TBoxLogin.Background = Brushes.White;
                BtnRegisterConfirm.IsEnabled = true;
            }
            else
            {
                TBoxLogin.Background = Brushes.LightPink;
                BtnRegisterConfirm.IsEnabled = false;
            }
        }

        readonly Regex nameRegex = new Regex(pattern: @"\w+\s\w+\s\w+");
        private void NameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (nameRegex.IsMatch(NameBox.Text))
            {
                NameBox.Background = Brushes.White;
                BtnRegisterConfirm.IsEnabled = true;
            }
            else
            {
                NameBox.Background = Brushes.LightPink;
                BtnRegisterConfirm.IsEnabled = false;
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterGrid.Visibility = Visibility.Visible;
            TBoxLogin_TextChanged(null, null);
            NameBox_TextChanged(null, null);
            PasswordBoxFirst_PasswordChanged(null, null);
            PasswordBoxSecond_PasswordChanged(null, null);
        }

        private void BtnRegisterClose_Click(object sender, RoutedEventArgs e)
        {
            RegisterGrid.Visibility = Visibility.Collapsed;
        }

        private void CheckPasswords()
        {
            if (passwordRegex.IsMatch(PasswordBoxFirst.Password))
            {
                PasswordBoxFirst.Background = Brushes.White;
                BtnRegisterConfirm.IsEnabled = true;
            }
            else
            {
                PasswordBoxFirst.Background = Brushes.LightPink;
                BtnRegisterConfirm.IsEnabled = false;
            }
            if (PasswordBoxSecond.Password == PasswordBoxFirst.Password
               && PasswordBoxFirst.Password.Length > 0
               && PasswordBoxSecond.Password.Length > 0)
            {
                PasswordBoxSecond.Background = Brushes.White;
                BtnRegisterConfirm.IsEnabled = true;
            }
            else
            {
                PasswordBoxSecond.Background = Brushes.LightPink;
                BtnRegisterConfirm.IsEnabled = false;
            }
        }

        readonly Regex passwordRegex = new Regex(pattern: @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,100}$");
        private void PasswordBoxFirst_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckPasswords();
        }

        private void PasswordBoxSecond_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckPasswords();
        }

        private void BtnRegisterConfirm_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (PasswordBoxFirst.Background == Brushes.White
                && PasswordBoxSecond.Background == Brushes.White
                && NameBox.Background == Brushes.White
                && TBoxLogin.Background == Brushes.White)
            {
                BtnRegisterConfirm.IsEnabled = true;
            }
            else
            {
                BtnRegisterConfirm.IsEnabled = false;
            }
        }
    }
}
