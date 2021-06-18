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
        /// <summary>
        /// Gets status list which can be selected via ComboBox
        /// through the master panel
        /// in the order DataGrid.
        /// </summary>
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
        /// <summary>
        /// Gets a price sum of the order.
        /// </summary>
        public string GetPrice
        {
            get
            {
                decimal sum = 0;
                List<Service> services = Service.ToList();
                List<ICollection<ServiceOfModel>> allModels = services.Select(s => s.ServiceOfModel).ToList();
                ICollection<ServiceOfModel> serviceOfModels = allModels.First();
                List<ServiceOfModel> contextModels = serviceOfModels.Where(m => m.T_ID.Equals(T_ID)).ToList();
                foreach (ServiceOfModel model in contextModels)
                {
                    sum += model.COST;
                }
                return sum == 0 ? "По договорённости" : sum + " руб.";
            }
        }
    }
}
