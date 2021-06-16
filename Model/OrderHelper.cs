using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CarRepairShopApp.Model
{
    public partial class Order
    {
        public string IsOrderChecked
        {
            get
            {
                return O_ISCHECKED ? "Подтвержден" : "Не подтвержден";
            }
        }
        public List<Status> GetStatusList
        {
            get
            {
                return Manager.Context.Status.ToList();
            }
        }
        public string CustomerName
        {
            get
            {
                return Client.Count() == 0 ? "Не указано" : Client.FirstOrDefault().CL_NAME;
            }
        }
        public string GetMasterNameOfOrder
        {
            get
            {
                return Master.Count == 0 ? "Не указано" : Master.FirstOrDefault().M_NAME;
            }
        }
        public string GetPrice
        {
            get
            {
                return Service.Sum(s => s.ServiceOfModel.Sum(t => t.COST)).ToString() + " руб.";
            }
        }
    }
}
