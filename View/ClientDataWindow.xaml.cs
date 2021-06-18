using CarRepairShopApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CarRepairShopApp.View
{
    /// <summary>
    /// Interaction logic for ClientDataWindow.xaml
    /// </summary>
    public partial class ClientDataWindow : Window
    {
        private readonly List<Service> _currentServices = new List<Service>();
        private readonly TypeOfCar _currentModel = new TypeOfCar();
        public ClientDataWindow(List<Service> services, TypeOfCar type)
        {
            InitializeComponent();
            _currentServices = services;
            _currentModel = type;
        }

        private void TBoxPassNum_KeyDown(object sender, KeyEventArgs e)
        {
            if ((!(e.Key >= Key.D0) || !(e.Key <= Key.D9)) && (e.Key != Key.Tab))
            {
                e.Handled = true;
                System.Media.SystemSounds.Asterisk.Play();
            }
            if (TBoxPassNum.Text.Length.Equals(4) && TBoxPassCode.Text.Length.Equals(6))
            {
                BtnSendRequest.IsEnabled = true;
            }
            else
            {
                BtnSendRequest.IsEnabled = false;
            }
        }

        private void BtnSendRequest_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Точно подтвердить подачу заявки на следующее количество услуг: "
                + _currentServices.Count
                + "?"
                + "\nТранзакцию нельзя отменить!",
                "Подтверждение заявки",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question)
                == MessageBoxResult.No)
            {
                return;
            }
            Order order = new Order();
            if (Manager.Context.Client.Where(c => c.CL_NAME.Equals(Manager.CurrentUser.USER_NAME)).Any().Equals(false))
            {
                Client client = new Client
                {
                    CL_NAME = Manager.CurrentUser.USER_NAME,
                    CL_PASSNUM = int.Parse(TBoxPassNum.Text),
                    CL_PASSCODE = int.Parse(TBoxPassCode.Text)
                };
                client.TypeOfCar.Add(_currentModel);
                order.TypeOfCar = _currentModel;
                Manager.Context.Client.Add(client);
                Manager.Context.SaveChanges();
            }
            order.O_CREATEDATE = DateTime.Now;
            Random random = new Random();
            order.Master.Add(Manager.Context.Master.ToList().OrderBy(m => random.Next()).First());
            order.Client.Add(Manager.Context.Client.Where(c => c.CL_NAME.Equals(Manager.CurrentUser.USER_NAME)).First());
            order.Status = Manager.Context.Status.Where(s => s.ST_STATE.Contains("в обработке")).FirstOrDefault();
            order.TypeOfCar = _currentModel;
            foreach (Service service in _currentServices)
            {
                order.Service.Add(service);
            }
            try
            {
                Manager.Context.Order.Add(order);
                Manager.Context.SaveChanges();
                MessageBox.Show("Заявка успешно подана!",
                    "Успешно!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Пожалуйста, попробуйте подать заявку ещё раз!",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void BtnDiscardChanges_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Точно отменить отправку заявки?",
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
