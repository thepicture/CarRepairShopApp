using CarRepairShopApp.Model;
using System;
using System.Collections.Generic;
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

        Regex costRegex = new Regex(pattern: @"[0-9]+");
        private void ModelCost_TextChanged(object sender, TextChangedEventArgs e)
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

        private void BtnAddSelectedModelsToService_Click(object sender, RoutedEventArgs e)
        {
            ModelGrid.SelectedItems.Cast<TypeOfCar>().ToList().ForEach(m => Manager.CurrentService.ServiceOfModel.Add(new ServiceOfModel
            {
                TypeOfCar = m,
                COST = decimal.Parse(ModelCost.Text)
            }));
        }
    }
}
