using CarRepairShopApp.Model;
using CarRepairShopApp.ViewModel;
using System;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CarRepairShopApp.View
{
    /// <summary>
    /// Interaction logic for AddEditCustomerWindow.xaml
    /// </summary>
    public partial class AddEditCustomerWindow : Window
    {
        private readonly Client _currentCustomer = new Client();
        public AddEditCustomerWindow(Client client)
        {
            InitializeComponent();
            if (client != null)
            {
                _currentCustomer = client;
            }
            DataContext = _currentCustomer;
        }

        /// <summary>
        /// Attaches an image to the customer.
        /// </summary>
        private void AttachCustomerPhoto_Click(object sender, RoutedEventArgs e)
        {
            if (PhotoGetter.OpenDialog())
            {
                _currentCustomer.CL_PHOTO = PhotoGetter.ImageInBytes;
                CustomerPhoto.Source = PhotoGetter.Image;
                PhotoGetter.SayAllOk();
            }
        }

        /// <summary>
        /// Opens the customer's cars window.
        /// </summary>
        private void BtnAddCarForCustomer_Click(object sender, RoutedEventArgs e)
        {
            Manager.CurrentCustomer = _currentCustomer;
            AddEditCustomerCarWindow carWindow = new AddEditCustomerCarWindow()
            {
                Title = "Модели автомобилей клиента " + _currentCustomer.CL_NAME
            };
            carWindow.ShowDialog();
        }

        /// <summary>
        /// Opens the customer's phone numbers window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddCustomerPhoneNumber_Click(object sender, RoutedEventArgs e)
        {
            Manager.CurrentCustomer = _currentCustomer;
            AddEditPhoneWindow phoneWindow = new AddEditPhoneWindow()
            {
                Title = "Номера телефонов клиента " + _currentCustomer.CL_NAME
            };
            phoneWindow.ShowDialog();
        }

        /// <summary>
        /// Saves the changes and closes the window when all is OK.
        /// </summary>
        private void BtnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            if (TBoxPassNum.Text.Length != 4)
            {
                builder.AppendLine("Укажите корректную серию паспорта");
            }
            if (TBoxPassCode.Text.Length != 6)
            {
                builder.AppendLine("Укажите корректный номер паспорта");
            }
            if (!RegexHelper.NameRegex.IsMatch(TBoxCustomerName.Text))
            {
                builder.AppendLine("Укажите корректное ФИО клиента");
            }
            if (builder.Length > 0)
            {
                MessageBox.Show(builder.ToString(),
                    Title + " неуспешно",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if (_currentCustomer.CL_ID.Equals(0))
            {
                Manager.Context.Client.Add(_currentCustomer);
            }
            try
            {
                Manager.Context.SaveChanges();
                MessageBox.Show("Данные о клиенте "
                    + _currentCustomer.CL_NAME
                    + " успешно сохранены!",
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

        /// <summary>
        /// Checks if the entered chars are digits. Tab is allowed to go to the next TextBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckPassportNumbers(object sender, KeyEventArgs e)
        {
            if ((!(e.Key >= Key.D0) || !(e.Key <= Key.D9)) && (e.Key != Key.Tab))
            {
                e.Handled = true;
                System.Media.SystemSounds.Asterisk.Play();
            }
        }

        /// <summary>
        /// Discards all the changes which were made to the customer.
        /// </summary>
        private void BtnDiscardChanges_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Действительно отменить "
                + Title
                +"?",
                "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                Manager.Context.Entry(_currentCustomer).State = System.Data.Entity.EntityState.Unchanged;
                Close();
            }
        }
    }
}
