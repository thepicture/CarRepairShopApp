namespace CarRepairShopApp.Model
{
    public partial class Contract
    {
        public string IsContractChecked
        {
            get
            {
                return CO_ISCHECKED ? "Подписан" : "Не подписан";
            }
        }
    }
}
