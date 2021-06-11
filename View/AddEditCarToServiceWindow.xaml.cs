using CarRepairShopApp.Model;
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
            ModelGrid.ItemsSource = Manager.Context.TypeOfCar.ToList();
            ServiceModelGrid.ItemsSource = Manager.CurrentService.ServiceOfModel.ToList();
        }


        private void ModelCost_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckIfTheCostAndSelectionAreValid();
        }

        private void BtnAddSelectedModelsToService_Click(object sender, RoutedEventArgs e)
        {
            Manager.CurrentService.ServiceOfModel.Add(new ServiceOfModel
            {
                T_ID = (ModelGrid.SelectedItem as TypeOfCar).T_ID,
                COST = decimal.Parse(ModelCost.Text)
            });
            ServiceModelGrid.ItemsSource = Manager.CurrentService.ServiceOfModel.ToList();
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
    }
}
