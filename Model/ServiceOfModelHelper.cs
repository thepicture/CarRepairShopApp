using System.Linq;

namespace CarRepairShopApp.Model
{
    public partial class ServiceOfModel
    {
        public string GetTypeName
        {
            get
            {
                return Manager.Context.TypeOfCar.Where(t => t.T_ID == T_ID).FirstOrDefault().T_NAME;
            }
        }
        public string GetCarName
        {
            get
            {
                return Manager.Context.Auto.Where(a => a.A_ID == Manager.Context.TypeOfCar.Where(t => t.T_ID == T_ID).FirstOrDefault().A_ID).FirstOrDefault().A_NAME;
            }
        }
    }
}
