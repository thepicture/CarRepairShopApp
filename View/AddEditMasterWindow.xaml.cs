using CarRepairShopApp.Model;
using CarRepairShopApp.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CarRepairShopApp.View
{
    /// <summary>
    /// Interaction logic for AddEditMasterWindow.xaml
    /// </summary>
    public partial class AddEditMasterWindow : Window
    {
        private Master _currentMaster = new Master();
        public AddEditMasterWindow(Master master)
        {
            InitializeComponent();
            if (master != null)
            {
                _currentMaster = master;
            }
            DataContext = _currentMaster;
        }

        private void BtnAddMasterPhoto_Click(object sender, RoutedEventArgs e)
        {
            if (PhotoGetter.OpenDialog())
            {
                _currentMaster.M_PHOTO = PhotoGetter.ImageInBytes;
                MasterPhoto.Source = PhotoGetter.Image;
                PhotoGetter.SayAllOk();
            }
        }

        private void BtnSaveMaster_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (!RegexHelper.NameRegex.IsMatch(MasterNameBox.Text))
            {
                errors.AppendLine("Укажите корректное ФИО механика");
            }
            if (!RegexHelper.PassCodeRegex.IsMatch(MasterPassCodeBox.Text))
            {
                errors.AppendLine("Укажите корректный код паспорта (6 цифр)");
            }
            if (!RegexHelper.NumberRegex.IsMatch(MasterExperienceBox.Text))
            {
                errors.AppendLine("Укажите корректное число лет стажа");
            }
            if (!RegexHelper.PassNumRegex.IsMatch(MasterPassNumBox.Text))
            {
                errors.AppendLine("Укажите корректную серию паспорта (4 цифры)");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(),
                    "Некорректные данные",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            if (_currentMaster.M_ID == 0)
            {
                Manager.Context.Master.Add(_currentMaster);
            }
            try
            {
                Manager.Context.SaveChanges();
                MessageBox.Show("Данные о мастере " + MasterNameBox.Text + " успешно обработаны!",
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
            if (MessageBox.Show("Действительно отменить редактирование механика?",
                "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                Manager.Context.Entry(_currentMaster).State = System.Data.Entity.EntityState.Unchanged;
                Close();
            }
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
            if (e.Source is MenuItem menuItem)
            {
                ButtonName.Text = menuItem.Header.ToString();
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
