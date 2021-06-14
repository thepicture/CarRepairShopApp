using CarRepairShopApp.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace CarRepairShopApp.View
{
    /// <summary>
    /// Interaction logic for AddEditCarToServiceWindow.xaml
    /// </summary>
    public partial class AddEditCarToServiceWindow : Window
    {
        public AddEditCarToServiceWindow()
        {
            InitializeComponent();
            UpdateDataGrids();
        }

        private void UpdateDataGrids()
        {
            ServiceModelGrid.ItemsSource = Manager.CurrentService.ServiceOfModel.ToList();
            List<string> servicesToRemove = Manager.CurrentService.ServiceOfModel.Select(s => s.TypeOfCar.T_NAME).ToList();
            ModelGrid.ItemsSource = Manager.Context.TypeOfCar.ToList().Where(t => !servicesToRemove.Contains(t.T_NAME)).ToList();
        }

        private void ModelCost_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckIfTheCostAndSelectionAreValid();
        }

        private void BtnAddSelectedModelsToService_Click(object sender, RoutedEventArgs e)
        {
            Manager.CurrentService.ServiceOfModel.Add(new ServiceOfModel
            {
                TypeOfCar = ModelGrid.SelectedItem as TypeOfCar,
                COST = decimal.Parse(ModelCost.Text)
            });
            UpdateDataGrids();
        }

        readonly Regex costRegex = new Regex(pattern: @"[1-9][0-9]{0,19}");
        private void CheckIfTheCostAndSelectionAreValid()
        {
            if (ModelGrid.SelectedItems.Count > 0 && costRegex.IsMatch(ModelCost.Text))
            {
                BtnAddSelectedModelsToService.IsEnabled = true;
            }
            else
            {
                BtnAddSelectedModelsToService.IsEnabled = false;
            }
        }

        private void ModelGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckIfTheCostAndSelectionAreValid();
        }

        private void BtnDeleteModelFromService_Click(object sender, RoutedEventArgs e)
        {
            Manager.CurrentService.ServiceOfModel.Remove(ServiceModelGrid.SelectedItem as ServiceOfModel);
            BtnDeleteModelFromService.IsEnabled = false;
            UpdateDataGrids();
        }

        private void ServiceModelGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnDeleteModelFromService.IsEnabled = true;
        }

        private void CloseEditCarToServiceWindow_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Точно закрыть окно указание стоимости услуги для моделей автомобилей?",
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
