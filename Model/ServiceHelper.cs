using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRepairShopApp.Model
{
    public partial class Service
    {
        public string GetServicePrice
        {
            get
            {
                return ServiceOfModel.Sum(s => s.COST).ToString() + " руб.";
            }
        }
    }
}
