using CarRepairShopApp.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CarRepairShopApp.View
{
    /// <summary>
    /// Interaction logic for AddEditPhoneWindow.xaml
    /// </summary>
    public partial class AddEditPhoneWindow : Window
    {
        public AddEditPhoneWindow()
        {
            InitializeComponent();
            PhonesGrid.ItemsSource = Manager.CurrentCustomer.Phone.ToList();
        }


        private void PhonesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnDeleteSelectedPhones.IsEnabled = true;
        }

        private void BtnAddPhone_Click(object sender, RoutedEventArgs e)
        {
            Manager.CurrentCustomer.Phone.Add(new Phone { P_NUMBER = TBoxPhoneNumber.Text });
            MessageBox.Show("Номер успешно добавлен!",
                "Успешно!",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            PhonesGrid.ItemsSource = Manager.CurrentCustomer.Phone.ToList();
        }

        private void BtnDeleteSelectedPhones_Click(object sender, RoutedEventArgs e)
        {
            List<Phone> phones = PhonesGrid.SelectedItems.Cast<Phone>().ToList();
            if (MessageBox.Show("Точно удалить следующее число номеров клиента: "
                + phones.Count
                + "?" +
                "\nЭто действие отменить невозможно!",
                "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning)
                == MessageBoxResult.Yes)
            {
                foreach (Phone phone in phones)
                {
                    Manager.CurrentCustomer.Phone.Remove(phone);
                }
                MessageBox.Show("Выбранные номера телефонов успешно удалены!",
                    "Успешно!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                PhonesGrid.ItemsSource = Manager.CurrentCustomer.Phone.ToList();
                BtnDeleteSelectedPhones.IsEnabled = false;
            }
        }

        private void TBoxPhoneNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if ((!(e.Key >= Key.D0) || !(e.Key <= Key.D9)) && (e.Key != Key.Tab))
            {
                e.Handled = true;
                System.Media.SystemSounds.Asterisk.Play();
            }
        }

        private void BtnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Действительно закрыть текущее окно?",
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
