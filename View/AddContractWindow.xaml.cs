using CarRepairShopApp.Model;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CarRepairShopApp.View
{
    /// <summary>
    /// Interaction logic for AddContractWindow.xaml
    /// </summary>
    public partial class AddContractWindow : Window
    {
        private Contract _currentContract = new Contract();
        public AddContractWindow(Contract contract)
        {
            InitializeComponent();
            if (contract != null)
            {
                _currentContract = contract;
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
            OpenFileDialog dialog = new OpenFileDialog();
            if ((bool)dialog.ShowDialog())
            {
                try
                {
                    ContractImage.Source = new BitmapImage(new Uri(dialog.FileName));
                    _currentContract.CO_IMAGESCAN = File.ReadAllBytes(dialog.FileName);
                    MessageBox.Show("Изображение успешно обновлено!",
                        "Успешно!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Пожалуйста, выберите изображение для прикрепления.",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void BtnSaveContract_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (ContractImage.Source is null)
            {
                errors.AppendLine("Пожалуйста, прикрепите скан договора");
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
                MessageBox.Show("Данные о контракте от даты " + ContractPicker.SelectedDate.ToString() + " успешно обработаны!",
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
                Close();
            }
        }
    }
}
