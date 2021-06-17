using System;
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
        public byte[] GetPhotoOfService
        {
            get
            {
                return PhotoOfService.Count.Equals(0) ? null : PhotoOfService.FirstOrDefault().PHOTO;
            }
        }
        public string GetMiddlePrice
        {
            get
            {
                if (ServiceOfModel.Count == 0)
                {
                    return "По договору";
                }
                return "Средняя цена: "
                    + Math.Round(ServiceOfModel.Sum(s => s.COST) / ServiceOfModel.Count, 0)
                    + " руб.";
            }
        }
    }
}
