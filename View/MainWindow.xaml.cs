using CarRepairShopApp.Model;
using CarRepairShopApp.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CarRepairShopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes the ComboBoxes.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            ComboLogin.ItemsSource = Manager.Context.User.ToList();
            /// A code above uses recurrent formula. The code loads all except Administrator role.
            ComboRole.ItemsSource = Manager.Context.Role.ToList().Take(Manager.Context.Role.ToList().Count).Reverse().Take(Manager.Context.Role.ToList().Count - 2).Reverse();
        }

        /// <summary>
        /// Closes the current application.
        /// </summary>
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Checks if the login is correct and passes to appropriate window with respect to users role.
        /// </summary>
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (ComboLogin.SelectedItem is User currentUser)
            {
                if (PBoxPassword.Password.Equals(currentUser.USER_PASSWORD))
                {
                    MessageBox.Show($"Вы успешно авторизованы, "
                        + currentUser.USER_NAME + "!",
                        "Успешно!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    Manager.CurrentUser = currentUser;
                    if (currentUser.Role.NAME.Equals("Администратор"))
                    {
                        AdminWindow adminWindow = new AdminWindow
                        {
                            Owner = this,
                            Title = currentUser.USER_NAME + " — панель администратора"
                        };
                        adminWindow.Show();
                        Hide();
                    }
                    else if (currentUser.Role.NAME.Equals("Механик"))
                    {
                        MasterWindow masterWindow = new MasterWindow
                        {
                            Owner = this,
                            Title = currentUser.USER_NAME + " — панель механика"
                        };
                        masterWindow.Show();
                        Hide();
                    }
                    else if (currentUser.Role.NAME.Equals("Клиент"))
                    {
                        CustomerWindow customerWindow = new CustomerWindow
                        {
                            Owner = this,
                            Title = currentUser.USER_NAME + " — просмотр услуг"
                        };
                        customerWindow.Show();
                        Hide();
                    }
                    Manager.MainLoginRegisterWindow = this;
                    PBoxPassword.Password = null;
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
        /// <summary>
        /// Identifies when the input login is correct and non-empty. Otherwise turns the TextBox background to red.
        /// </summary>
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

        /// <summary>
        /// Identifies when the NameBox is correct and non-empty. Otherwise turns the TextBox background to red.
        /// </summary>
        private void NameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (RegexHelper.NameRegex.IsMatch(NameBox.Text))
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

        /// <summary>
        /// Opens the register panel.
        /// </summary>
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

        /// <summary>
        /// Closes the register panel.
        /// </summary>
        private void BtnRegisterClose_Click(object sender, RoutedEventArgs e)
        {
            CloseRegistration();
        }

        /// <summary>
        /// Checks if the first and second passwords are correct with respect to regex expression.
        /// </summary>
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

        /// <summary>
        /// Identifies when all of the TextBoxes are correct and non-empty. Otherwise turns all of the TextBoxes backgrounds to red.
        /// </summary>
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

        /// <summary>
        /// Register a new user.
        /// </summary>
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

        /// <summary>
        /// Closes current window.
        /// </summary>
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

        /// <summary>
        /// Opens a window to recover the password.
        /// </summary>
        private void BtnForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            RecoveryPasswordPanel.Visibility = Visibility.Visible;
            TBoxRecoveryLogin_TextChanged(null, null);
            Title = "Восстановление пароля в системе";
            LoginPanel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Identifies when the recovery login is correct and non-empty. Otherwise turns the TextBox background to red.
        /// </summary>
        private void TBoxRecoveryLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (loginRegex.IsMatch(TBoxRecoveryLogin.Text))
            {
                TBoxRecoveryLogin.Background = Brushes.Transparent;
                BtnRecover.IsEnabled = true;
            }
            else
            {
                TBoxRecoveryLogin.Background = Brushes.LightPink;
                BtnRecover.IsEnabled = false;
            }
        }

        /// <summary>
        /// Gives the recovered password if it exists. Otherwise shows the error message.
        /// </summary>
        private void BtnRecover_Click(object sender, RoutedEventArgs e)
        {
            User user = Manager.Context.User.ToList().Where(u => u.USER_LOGIN.Equals(TBoxRecoveryLogin.Text)).FirstOrDefault();
            if (user != null)
            {
                MessageBox.Show("Ваш пароль: " + user.USER_PASSWORD
                    + "\nПожалуйста, запишите его в безопасное место!",
                    "Пароль восстановлен!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                CloseRecoverySection();
            }
            else
            {
                MessageBox.Show("Пользователь в системе с логином "
                    + TBoxRecoveryLogin.Text
                    + " не найден!\n" +
                    "Проверьте правильность ввода или убедитесь, что пользователь зарегистрирован в системе.",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Closes the password recovery section.
        /// </summary>
        private void BtnCloseRecovery_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Точно покинуть окно восстановления пароля?\n" +
              "Введённые данные будут утеряны!",
              "Внимание",
              MessageBoxButton.YesNo,
              MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                CloseRecoverySection();
            }
        }

        /// <summary>
        /// Actions for returning to the main window.
        /// </summary>
        private void CloseRecoverySection()
        {

            RecoveryPasswordPanel.Visibility = Visibility.Collapsed;
            LoginPanel.Visibility = Visibility.Visible;
            Title = "Авторизация в системе";
            TBoxRecoveryLogin.Text = null;
        }

        /// <summary>
        /// Asks if user wants to close current application.
        /// </summary>
        private void LoginRegisterRecoveryWindow_Closing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Точно выйти из программы?",
                "Предупреждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question)
                != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Shows hints in the StatusBar.
        /// </summary>
        private void MainGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Source is Button button)
            {
                if (button.ToolTip != null)
                {
                    ButtonName.Text = button.ToolTip.ToString();
                }
                else
                {
                    ButtonName.Text = button.Content.ToString();
                }
            }
        }

        /// <summary>
        /// Make the StatusBar empty.
        /// </summary>
        private void MainGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonName.Text = null;
        }
    }
}
