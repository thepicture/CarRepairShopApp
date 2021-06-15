using CarRepairShopApp.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CarRepairShopApp.View
{
    /// <summary>
    /// Interaction logic for AddEditCustomerCarWindow.xaml
    /// </summary>
    public partial class AddEditCustomerCarWindow : Window
    {
        public AddEditCustomerCarWindow()
        {
            InitializeComponent();
            UpdateDataGridsAndComboBox();
            ComboModel.SelectedIndex = 0;
        }

        private void UpdateDataGridsAndComboBox()
        {
            CustomerModelsGrid.ItemsSource = Manager.CurrentCustomer.Auto.ToList();
            ModelGrid.ItemsSource = Manager.Context.TypeOfCar.ToList();
            ComboModel.ItemsSource = Manager.Context.TypeOfCar.ToList();
        }

        private void ModelGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BtnAddNewModel_Click(object sender, RoutedEventArgs e)
        {
            Manager.Context.TypeOfCar.Add(new TypeOfCar
            {
                Auto = ComboModel.SelectedItem as Auto,
                T_NAME = ModelName.Text
            });
            try
            {
                Manager.Context.SaveChanges();
                UpdateDataGridsAndComboBox();
            }
            catch (Exception)
            {
                MessageBox.Show("При сохранении новой модели произошла ошибка!" +
                    "\nПожалуйста, попробуйте добавить новую модель ещё раз.",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void BtnAddSelectedModelsToCustomer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseCarOfCustomerWindow_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Точно закрыть окно указания автомобилей клиента?",
              "Внимание",
              MessageBoxButton.YesNo,
              MessageBoxImage.Question)
              == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        private void CustomerModelsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BtnDeleteModelsOfCustomer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ModelName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ModelName.Text.Length > 0 || ComboModel.SelectedItem is null)
            {
                BtnAddNewModel.IsEnabled = true;
            }
            else
            {
                BtnAddNewModel.IsEnabled = false;
            }
        }
    }
}
