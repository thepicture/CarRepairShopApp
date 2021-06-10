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
            ComboRole.ItemsSource = Manager.Context.Role.ToList().Take(3).Reverse().Take(2).Reverse();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Точно выйти из программы?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                App.Current.Shutdown();
            }
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
                TBoxLogin.Background = Brushes.Transparent;
                BtnRegisterConfirm.IsEnabled = true;
            }
            else
            {
                TBoxLogin.Background = Brushes.LightPink;
                BtnRegisterConfirm.IsEnabled = false;
            }
            UpdateState();
        }

        readonly Regex nameRegex = new Regex(pattern: @"\w+\s\w+\s\w+");
        private void NameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (nameRegex.IsMatch(NameBox.Text))
            {
                NameBox.Background = Brushes.Transparent;
                BtnRegisterConfirm.IsEnabled = true;
            }
            else
            {
                NameBox.Background = Brushes.LightPink;
                BtnRegisterConfirm.IsEnabled = false;
            }
            UpdateState();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterPanel.Visibility = Visibility.Visible;
            TBoxLogin_TextChanged(null, null);
            NameBox_TextChanged(null, null);
            PasswordBoxFirst_PasswordChanged(null, null);
            PasswordBoxSecond_PasswordChanged(null, null);
            Title = "Регистрация в системе";
            LoginPanel.Visibility = Visibility.Collapsed;
        }

        private void BtnRegisterClose_Click(object sender, RoutedEventArgs e)
        {
            CloseRegistration();
        }

        private void CheckPasswords()
        {
            if (passwordRegex.IsMatch(PasswordBoxFirst.Password))
            {
                PasswordBoxFirst.Background = Brushes.Transparent;
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
                PasswordBoxSecond.Background = Brushes.Transparent;
                BtnRegisterConfirm.IsEnabled = true;
            }
            else
            {
                PasswordBoxSecond.Background = Brushes.LightPink;
                BtnRegisterConfirm.IsEnabled = false;
            }
            UpdateState();
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

        private void UpdateState()
        {
            if (PasswordBoxFirst.Background == Brushes.Transparent
                && PasswordBoxSecond.Background == Brushes.Transparent
                && NameBox.Background == Brushes.Transparent
                && TBoxLogin.Background == Brushes.Transparent)
            {
                BtnRegisterConfirm.IsEnabled = true;
            }
            else
            {
                BtnRegisterConfirm.IsEnabled = false;
            }
        }

        private void BtnRegisterConfirm_Click(object sender, RoutedEventArgs e)
        {
            User user = new User
            {
                USER_LOGIN = TBoxLogin.Text,
                USER_NAME = NameBox.Text,
                USER_PASSWORD = PasswordBoxFirst.Password,
                Role = ComboRole.SelectedItem as Role
            };
            Manager.Context.User.Add(user);
            Manager.Context.SaveChanges();
            MessageBox.Show("Пользователь " + user.USER_NAME +
                " успешно зарегистрирован!", "Успешно!",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            CloseRegistration();
        }

        private void CloseRegistration()
        {
            if (MessageBox.Show("Точно покинуть окно регистрации?\n" +
                "Введённые данные будут утеряны!",
                "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                RegisterPanel.Visibility = Visibility.Collapsed;
                LoginPanel.Visibility = Visibility.Visible;
                Title = "Авторизация в системе";
                TBoxLogin.Text = NameBox.Text = PasswordBoxFirst.Password = PasswordBoxSecond.Password = null;
                ComboRole.SelectedIndex = 0;
            }
        }
    }
}
