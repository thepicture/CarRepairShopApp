using System;

namespace CarRepairShopApp.Model
{
    public partial class Order
    {
        public string CheckedStatus
        {
            get
            {
                return O_ISCHECKED ? "Подтвержден" : "Не подтвержден";
            }
        }
    }
}
