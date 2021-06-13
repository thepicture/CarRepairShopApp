using CarRepairShopApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            }
            DataContext = _currentOrder;
        }
    }
}
