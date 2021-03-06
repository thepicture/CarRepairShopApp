using CarRepairShopApp.Model;
using System;
using System.Linq;
using System.Text;
using System.Windows;

namespace CarRepairShopApp.View
{
    /// <summary>
    /// Interaction logic for AddEditOrderWindow.xaml
    /// </summary>
    public partial class AddEditOrderWindow : Window
    {
        private readonly Order _currentOrder = new Order();
        public AddEditOrderWindow(Order order)
        {
            InitializeComponent();
            if (order != null)
            {
                _currentOrder = order;
                GetNameAndCarOfTheCustomerIfExists();
            }
            DataContext = _currentOrder;
            TBoxMasterName.Text = Manager.CurrentUser.USER_NAME;
        }

        private void GetNameAndCarOfTheCustomerIfExists()
        {
            if (_currentOrder.Client.Count > 0)
            {
                CustomerName.Text = _currentOrder.Client.FirstOrDefault().CL_NAME;
                ComboCar.ItemsSource = _currentOrder.Client.FirstOrDefault().TypeOfCar.ToList();
                ComboCar.SelectedItem = _currentOrder.TypeOfCar;
                TBoxMasterName.Text = Manager.CurrentUser.USER_NAME;
            }
        }

        private void BtnSelectCustomer_Click(object sender, RoutedEventArgs e)
        {
            Manager.CurrentOrder = _currentOrder;
            SelectCustomerWindow customerWindow = new SelectCustomerWindow
            {
                Title = "Выбор клиента"
            };
            customerWindow.ShowDialog();
            GetNameAndCarOfTheCustomerIfExists();
        }

        private void BtnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            if (CustomerName.Text.Length.Equals(0))
            {
                builder.AppendLine("Укажите клиента");
            }
            if (ComboCar.SelectedItem is null)
            {
                builder.AppendLine("Укажите автомобиль клиента");
            }
            if (builder.Length > 0)
            {
                MessageBox.Show(builder.ToString(),
                    Title + " неуспешно",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            Master master = Manager.Context.Master.Where(m => m.M_NAME.Equals(Manager.CurrentUser.USER_NAME)).FirstOrDefault();
            if (master is null)
            {
                _currentOrder.Master.Add(new Master
                {
                    M_NAME = Manager.CurrentUser.USER_NAME
                });
            }
            else
            {
                _currentOrder.Master.Add(master);
            }
            _currentOrder.TypeOfCar = ComboCar.SelectedItem as TypeOfCar;
            if (_currentOrder.O_ID.Equals(0))
            {
                _currentOrder.Status = Manager.Context.Status.First(s => s.ST_STATE.Equals("Не оплачен"));
                Manager.Context.Order.Add(_currentOrder);
            }
            try
            {
                Manager.Context.SaveChanges();
                MessageBox.Show("Заказ клиента "
                    + _currentOrder.Client.FirstOrDefault().CL_NAME
                    + " успешно сохранен!",
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

        private void BtnServiceSelect_Click(object sender, RoutedEventArgs e)
        {
            Manager.CurrentOrder = _currentOrder;
            AddServiceToOrderWindow serviceToOrderWindow = new AddServiceToOrderWindow
            {
                Title = "Выбор услуг в текущий заказ"
            };
            serviceToOrderWindow.ShowDialog();
        }

        private void BtnDiscardChanges_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Точно отменить "
                + Title
                + "?",
                "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                Manager.Context.Entry(_currentOrder).State = System.Data.Entity.EntityState.Unchanged;
                Close();
            }
        }

        private void ComboCar_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (!ComboCar.SelectedItem.Equals(null))
            {
                _currentOrder.TypeOfCar = ComboCar.SelectedItem as TypeOfCar;
                BtnServiceSelect.IsEnabled = true;
            }
        }
    }
}
