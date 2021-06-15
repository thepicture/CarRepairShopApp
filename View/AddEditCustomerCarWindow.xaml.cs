using CarRepairShopApp.Model;
using CarRepairShopApp.ViewModel;
using System;
using System.Collections.Generic;
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
            CustomerModelsGrid.ItemsSource = Manager.CurrentCustomer.TypeOfCar.ToList();
            ModelGrid.ItemsSource = Manager.Context.TypeOfCar.ToList().Except(Manager.CurrentCustomer.TypeOfCar).ToList();
            ComboModel.ItemsSource = Manager.Context.Auto.ToList();
        }

        private void ModelGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnAddSelectedModelsToCustomer.IsEnabled = true;
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
                MessageBox.Show("Модель автомобиля успешно добавлена!",
                    "Успешно!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
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
            List<TypeOfCar> models = ModelGrid.SelectedItems.Cast<TypeOfCar>().ToList();
            foreach (TypeOfCar model in models)
            {
                if (MessageBox.Show("Добавить дополнительно фотографию автомобиля клиента для "
                     + model.Auto.A_NAME + " " + model.T_NAME
                    + "?",
                    "Добавление новой модели автомобиля",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question)
                    == MessageBoxResult.Yes)
                {
                    if (PhotoGetter.OpenDialog())
                    {
                        model.Auto.A_PHOTO = PhotoGetter.ImageInBytes;
                    }
                }
                Manager.CurrentCustomer.TypeOfCar.Add(model);
            }
            UpdateDataGridsAndComboBox();
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
            BtnDeleteModelsOfCustomer.IsEnabled = true;
        }

        private void BtnDeleteModelsOfCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Действительно удалить выбранные автомобили клиента?",
                "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                foreach (TypeOfCar model in CustomerModelsGrid.SelectedItems)
                {
                    Manager.CurrentCustomer.TypeOfCar.Remove(model);
                }
            }
            try
            {
                Manager.Context.SaveChanges();
                MessageBox.Show("Операция успешна!",
                   "Успешно!",
                   MessageBoxButton.OK,
                   MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("При удалении модели клиента произошла ошибка!" +
                   "\nПожалуйста, попробуйте удалить выбранные модели ещё раз.",
                   "Ошибка",
                   MessageBoxButton.OK,
                   MessageBoxImage.Error);
            }
            UpdateDataGridsAndComboBox();
            BtnDeleteModelsOfCustomer.IsEnabled = false;
        }

        private void ModelName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ModelName.Text.Length > 0 || !(ComboModel.SelectedItem is null))
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
