namespace CarRepairShopApp.Model
{
    public partial class TypeOfCar
    {
        public string GetModelAndAutoName
        {
            get
            {
                return Auto.A_NAME + " " + T_NAME;
            }
        }
    }
}
