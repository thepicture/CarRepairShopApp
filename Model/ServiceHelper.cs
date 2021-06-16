using System.Linq;

namespace CarRepairShopApp.Model
{
    public partial class Service
    {
        public string GetServicePrice
        {
            get
            {
                var order = Manager.CurrentOrder.TypeOfCar;
                return ServiceOfModel.Where(m => Manager.CurrentOrder.TypeOfCar
                .Equals(m.TypeOfCar)).Sum(s => s.COST).Equals(0) ? "услуга недоступна" : ServiceOfModel.Where(m => Manager.CurrentOrder.TypeOfCar
                .Equals(m.TypeOfCar)).Sum(s => s.COST).ToString() + " руб.";
            }
        }
    }
}
