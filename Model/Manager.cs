﻿namespace CarRepairShopApp.Model
{
    class Manager
    {
        public static CarRepairShopBaseEntities Context = new CarRepairShopBaseEntities();
        public static User CurrentUser = new User();
        public static MainWindow MainLoginRegisterWindow = new MainWindow();
        public static Service CurrentService = new Service();
        public static Contract CurrentContract = new Contract();
    }
}
