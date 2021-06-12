using System;
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
        public string GetOrderStatus
        {
            get
            {
                return Status.ST_STATE;
            }
        }
        public string GetClientNameOfOrder
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
    }
}
