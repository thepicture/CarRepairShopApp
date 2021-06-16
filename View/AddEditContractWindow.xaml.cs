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
    /// Interaction logic for AddEditContractWindow.xaml
    /// </summary>
    public partial class AddEditContractWindow : Window
    {
        private readonly Contract _currentContract = new Contract();
        public AddEditContractWindow(Contract contract)
        {
            InitializeComponent();
            if (contract != null)
            {
                _currentContract = contract;
            }
            else
            {
                _currentContract.CO_DATE = DateTime.Now;
            }
            DataContext = _currentContract;
        }

        /// <summary>
        /// Shows hints in the StatusBar.
        /// </summary>
        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Source is Button button)
            {
                ButtonName.Text = button.Content.ToString();
            }
        }

        /// <summary>
        /// Make the StatusBar empty.
        /// </summary>
        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonName.Text = null;
        }

        private void BtnAddContractImage_Click(object sender, RoutedEventArgs e)
        {
            if (PhotoGetter.OpenDialog())
            {
                _currentContract.CO_IMAGESCAN = PhotoGetter.ImageInBytes;
                ContractImage.Source = PhotoGetter.Image;
                PhotoGetter.SayAllOk();
            }
        }

        private void BtnSaveContract_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (_currentContract.CO_IMAGESCAN is null)
            {
                errors.AppendLine("Пожалуйста, прикрепите скан договора.");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(),
                    "Некорректные данные",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            if (_currentContract.CO_ID == 0)
            {
                Manager.Context.Contract.Add(_currentContract);
            }
            try
            {
                Manager.Context.SaveChanges();
                MessageBox.Show("Данные о контракте от даты "
                    + ContractPicker.SelectedDate.ToString()
                    + " успешно обработаны!",
                    "Успешно!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                Manager.Context.ChangeTracker.Entries().ToList().ForEach(m => m.Reload());
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при сохранении! Пожалуйста, попробуйте ещё раз." +
                    "\nСообщение об ошибке: "
                    + ex.Message,
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Действительно отменить " + Title.ToLower() + "?",
                "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                Manager.Context.Entry(_currentContract).State = System.Data.Entity.EntityState.Unchanged;
                Close();
            }
        }

        private void BtnAddContractToCustomer_Click(object sender, RoutedEventArgs e)
        {
            Manager.CurrentContract = _currentContract;
            AddContractToCustomerWindow addContractToCustomerWindow = new AddContractToCustomerWindow()
            {
                Title = "Назначить контракт от " + _currentContract.CO_DATE + " клиенту"
            };
            addContractToCustomerWindow.ShowDialog();
        }
    }
}
